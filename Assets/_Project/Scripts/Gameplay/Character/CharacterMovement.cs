using System;
using PurrNet;
using Reflex.Attributes;
using Scripts.Gameplay.CameraFeature;
using UnityEngine;

namespace Scripts.Gameplay.Character
{
    public class CharacterMovement : NetworkBehaviour
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float walkSpeed = 2f;
        [SerializeField] private float runSpeed = 4f;
        [SerializeField] private float jumpHeight = 1.5f;
        [SerializeField] private float rotationSpeed = 10f;

        private Vector3 _velocity;
        private ICameraService _cameraService;
        private Transform cameraTransform;

        private bool _isGrounded;
        public bool IsGrounded => _isGrounded;

        public event Action OnGrounded;

        [Inject]
        private void Constructor(ICameraService cameraService)
        {
            _cameraService = cameraService;
            cameraTransform = _cameraService.MainCamera.transform;
        }

        public void Move(Vector3 moveInputRaw, bool isRunning)
        {
            if (cameraTransform == null) return;
            var speed = isRunning ? runSpeed : walkSpeed;

            var cameraForward = cameraTransform.forward;
            var cameraRight = cameraTransform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;

            cameraForward.Normalize();
            cameraRight.Normalize();

            var inputMove = cameraForward * moveInputRaw.y + cameraRight * moveInputRaw.x;

            var finalMove = inputMove * speed + Vector3.up * _velocity.y;
            controller.Move(finalMove * Time.deltaTime);
        }

        public void ApplyGravity()
        {
            if (controller.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            _velocity.y += gravity * Time.deltaTime;
        }

        public void UpdateIsGrounded()
        {
            if (isOwner)
            {
                var grounded = controller.isGrounded;
                if (grounded != _isGrounded)
                {
                    if (grounded)
                    {
                        OnGrounded?.Invoke();
                    }

                    _isGrounded = grounded;
                    SendGroundStateToServer(_isGrounded);
                }
            }
        }

        public void Jump()
        {
            if (controller.isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        public void RotateCharacterToCameraDirection()
        {
            if (cameraTransform == null) return;
            var cameraForward = _cameraService.MainCamera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            if (cameraForward != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(cameraForward);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }

        [ServerRpc]
        private void SendGroundStateToServer(bool grounded)
        {
            _isGrounded = grounded;
            BroadcastGroundState(grounded);
        }

        [ObserversRpc]
        private void BroadcastGroundState(bool grounded)
        {
            _isGrounded = grounded;
        }
    }
}