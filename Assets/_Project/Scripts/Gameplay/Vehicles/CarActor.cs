using System;
using PurrNet;
using Reflex.Attributes;
using Scripts.Gameplay.Character;
using Scripts.Gameplay.Collectables;
using Scripts.Gameplay.Vehicles.DriveComponents;
using Scripts.Infrastructure.Services;
using UnityEngine;

namespace Scripts.Gameplay.Vehicles
{
    public class CarActor : NetworkBehaviour, ICollector
    {
        [Header("Car components")]
        [SerializeField] private CarLights lights;
        [SerializeField] private CarWheels wheels;

        private CarControl _carControl;
        private InputModeSwitcher _inputModeSwitcher;
        private CharacterActor _character;
        public float Speed { get; set; }
        public event Action<float> OnSpeedChanged;

        [Inject]
        private void Constructor(InputModeSwitcher inputModeSwitcher)
        {
            _inputModeSwitcher = inputModeSwitcher;
        }

        private void Start()
        {
            _carControl = GetComponent<CarControl>();
            lights.ShowFront(false);
            lights.ShowBack(false);
        }

        public void SetOwner(CharacterActor character)
        {
            if (_character != null)
            {
                Debug.Log("Only one driver allowed");
                return;
            }
            // GiveOwnership();
            _character = character;
            var input = character.GetComponent<CharacterInput>();
            _character.Model.gameObject.SetActive(false);
            character.transform.position = Vector3.down * 70;
            _carControl.SetInput(input.InputMap.Car);
            _carControl.SetActive(true);
            lights.ShowFront(true);
        }

        public void Exit()
        {
            // SetOwner(null);
            _character.transform.position = transform.position - transform.right * 2; //TODO check if no blocked
            _character.Model.gameObject.SetActive(true);
            _inputModeSwitcher.ExitCar(_character);
            _carControl.SetActive(false);
            _character = null;

            lights.ShowFront(false);
        }

        private void Update()
        {
            var newSpeed = _carControl.RigidBody.linearVelocity.sqrMagnitude;

            if (!Mathf.Approximately(newSpeed, Speed))
            {
                Speed = newSpeed;
                OnSpeedChanged?.Invoke(Speed);
            }
        }
    }
}