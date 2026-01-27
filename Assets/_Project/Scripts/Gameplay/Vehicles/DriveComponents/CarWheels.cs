using System.Collections.Generic;
using PurrNet;
using UnityEngine;

namespace Scripts.Gameplay.Vehicles.DriveComponents
{
    public class CarWheels : NetworkBehaviour
    {
        [SerializeField] private Rigidbody rb;

        [Header("Skidmark Thresholds")]
        [SerializeField] private float slipThreshold = 0.3f;

        [SerializeField] private float velocityThreshold = 5f;
        [SerializeField] private WheelControl[] wheels;
        [SerializeField] private List<TrailRenderer> skidmarks;
        public float steeringRange = 30f;
        public float steeringRangeAtMaxSpeed = 10f;

        public WheelControl[] Wheels => wheels;

        protected override void OnSpawned()
        {
            base.OnSpawned();
            if (localPlayer.HasValue)
            {
                if (!localPlayer.Value.isServer)
                {
                    foreach (var wc in wheels)
                        wc.WheelCollider.enabled = false;
                }
            }
        }

        private void FixedUpdate()
        {
            for (var i = 0; i < wheels.Length; i++)
            {
                wheels[i].WheelCollider.GetGroundHit(out var hit);
                var shouldSkid = ShouldShowSkidmark(hit) && wheels[i].WheelCollider.isGrounded;
                skidmarks[i].emitting = shouldSkid;
            }
        }

        public void Motorize(float torque)
        {
            foreach (var wheel in wheels)
            {
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = torque;
                }

                wheel.WheelCollider.brakeTorque = 0f;
            }
        }

        public void Steer(float hInput, float speedFactor)
        {
            var currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);
            foreach (var wheel in wheels)
            {
                if (wheel.steerable)
                {
                    wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
                }
            }
        }

        public void ResetSteer()
        {
            foreach (var wheel in wheels)
            {
                if (wheel.steerable)
                {
                    wheel.WheelCollider.steerAngle = 0f;
                }
            }
        }

        private bool ShouldShowSkidmark(WheelHit hit)
        {
            if (hit.collider == null)
                return false;

            // when too slow
            if (rb.linearVelocity.magnitude < velocityThreshold)
                return false;

            var forwardSlip = Mathf.Abs(hit.forwardSlip);
            var sidewaysSlip = Mathf.Abs(hit.sidewaysSlip);
            var totalSlip = Mathf.Sqrt(forwardSlip * forwardSlip + sidewaysSlip * sidewaysSlip);

            return totalSlip > slipThreshold;
        }
    }
}