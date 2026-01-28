using PurrNet;
using UnityEngine;

namespace Scripts.Gameplay.Character
{
    public class CharacterAnimatorSyncer : MonoBehaviour
    {
        [SerializeField] private NetworkAnimator networkAnimator;
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterInput input;
        [SerializeField] private CharacterMovement movement;

        [Header("Smoothing")]
        [SerializeField] private float movementDampTime = 0.1f;
        [SerializeField] private float speedDampTime = 0.1f;

        private static readonly int Speed = Animator.StringToHash("speed");
        private static readonly int MoveY = Animator.StringToHash("moveY");
        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
        private static readonly int JumpHash = Animator.StringToHash("jump");

        private void Update()
        {
            if (!networkAnimator.isOwner)
                return;
            
            var speed = Mathf.Abs(input.MoveInput.magnitude);
            animator.SetFloat(Speed, speed, speedDampTime, Time.deltaTime);
            networkAnimator.SetFloat(Speed, speed);

            var multiplier = input.IsRun ? 2 : 1;
            var inputDirection = input.MoveInput * multiplier;
            animator.SetFloat(MoveX, inputDirection.x, movementDampTime, Time.deltaTime);
            networkAnimator.SetFloat(MoveX, inputDirection.x);

            animator.SetFloat(MoveY, inputDirection.y, movementDampTime, Time.deltaTime);
            networkAnimator.SetFloat(MoveY, inputDirection.y);
            
            animator.SetBool(IsGrounded, movement.IsGrounded);
            networkAnimator.SetBool(IsGrounded, movement.IsGrounded);
        }

        public void Jump()
        {
            animator.SetTrigger(JumpHash);
            networkAnimator.SetTrigger(JumpHash);
        }
    }
}