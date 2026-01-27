using PurrNet;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using Scripts.Gameplay.CameraFeature;
using Scripts.Gameplay.Collectables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Gameplay.Character
{
    public class CharacterActor : NetworkBehaviour, ICollector
    {
        [SerializeField] private GameObject model;
        [SerializeField] private Interactor interactor;
        [SerializeField] private CharacterInput input;
        [SerializeField] private CharacterMovement movement;
        [SerializeField] private CharacterAnimatorSyncer animatorSyncer;
        private ICameraService _cameraService;

        public CharacterInput Input => input;
        public GameObject Model => model;

        [Inject]
        private void Constructor(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }

        protected override void OnSpawned()
        {
            base.OnSpawned();
            GameObjectInjector.InjectRecursive(gameObject, SceneManager.GetActiveScene().GetSceneContainer());
            if (isOwner)
            {
                // TakeOwnership();
                _cameraService.SetTarget(transform);
                input.ActivateCharacter(true);
            }
            else
            {
                input.ActivateCharacter(false);
            }
        }

        private void OnValidate()
        {
            interactor ??= GetComponent<Interactor>();
            input ??= GetComponent<CharacterInput>();
            movement ??= GetComponent<CharacterMovement>();
            animatorSyncer ??= GetComponent<CharacterAnimatorSyncer>();
        }

        private void Start()
        {
            input.ActivateCharacter(true);
            if (isOwner)
                movement.OnGrounded += OnGroundedHandler;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (movement != null)
                movement.OnGrounded -= OnGroundedHandler;
        }

        private void Update()
        {
            movement.UpdateIsGrounded();
            movement.ApplyGravity();
            if (!isOwner) return;

            interactor.Tick();
            input.ReadValues();
            HandleMovement();
            HandleJump();
            HandleInteract();
        }

        private void HandleMovement()
        {
            movement.Move(input.MoveInput, input.IsRun);
            movement.RotateCharacterToCameraDirection();
        }

        private void HandleJump()
        {
            if (input.IsJumped)
            {
                movement.Jump();
                animatorSyncer.Jump();
            }
        }

        private void HandleInteract()
        {
            if (input.IsInteracted)
            {
                if (TryGetComponent(out Interactor interactor))
                {
                    interactor.Interact();
                }
            }
        }

        private void OnGroundedHandler()
        {
            _cameraService.Shake(0.2f);
        }
    }
}