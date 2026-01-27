using System;
using Scripts.Gameplay.Vehicles.DriveComponents;
using UnityEngine;

namespace Scripts.Gameplay.Vehicles
{
    public class CarControl : MonoBehaviour
    {
        [SerializeField] private CarWheels wheels;

        [Header("Car Properties")]
        public float motorTorque = 2000f;

        public float brakeTorque = 2000f;
        public float maxSpeed = 20f;
        public float centreOfGravityOffset = -1f;

        private Rigidbody rigidBody;
        private InputSystem_Actions.CarActions input;
        private bool _isActive;
        private bool _isBreaking;
        private Vector2 _moveInput;
        private bool _hasInput;
        private bool _reverseAccelerate;

        public Rigidbody RigidBody => rigidBody;
        public bool IsBreaking => _isBreaking;
        public bool ReverseAccelerate => _reverseAccelerate;

        public void SetInput(InputSystem_Actions.CarActions inputParam)
        {
            input = inputParam;
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            var centerOfMass = rigidBody.centerOfMass;
            centerOfMass.y += centreOfGravityOffset;
            rigidBody.centerOfMass = centerOfMass;
        }

        private void Update()
        {
            if (!_isActive) return;

            _moveInput = input.Move.ReadValue<Vector2>();
            _isBreaking = input.Break.IsPressed();

            if (input.Exit.WasPressedThisFrame())
            {
                GetComponent<CarActor>().Exit();
            }
        }


        private void FixedUpdate()
        {
            if (!_isActive)
            {
                StopCar(20);
                return;
            }

            var hInput = _moveInput.x;
            var vInput = _moveInput.y;
            _reverseAccelerate = vInput < 0;

            var forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);
            var speedFactor = Mathf.InverseLerp(0, maxSpeed, Mathf.Abs(forwardSpeed));
            var currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor); //reduce at high speed

            wheels.Steer(hInput, speedFactor);

            var isAccelerating = Math.Abs(Mathf.Sign(vInput) - Mathf.Sign(forwardSpeed)) < 0.01f;
            var hasInput = Mathf.Abs(vInput) > 0.01f || Mathf.Abs(hInput) > 0.01f;

            if (_isBreaking)
            {
                StopCar(5);
            }
            else if (isAccelerating && hasInput)
            {
                wheels.Motorize(vInput * currentMotorTorque);
            }
            else if (hasInput)
            {
                StopCar();
            }
            else
            {
                wheels.ResetSteer();
                StopCar(0.6f);
            }
        }

        private void StopCar(float breakPower = 1f)
        {
            foreach (var wheel in wheels.Wheels)
            {
                wheel.WheelCollider.motorTorque = 0f;
                wheel.WheelCollider.brakeTorque = brakeTorque * breakPower;
                // wheel.WheelCollider.steerAngle = 0f;
            }
        }
    }
}