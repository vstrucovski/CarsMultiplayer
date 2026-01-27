using UnityEngine;

namespace Scripts.Gameplay.Vehicles.DriveComponents
{
    public class WheelControl : MonoBehaviour
    {
        [field: SerializeField] public WheelCollider WheelCollider { get; private set; }
        [SerializeField] private Transform wheelModel;
        [field: SerializeField] public bool steerable { get; private set; }
        [field: SerializeField] public bool motorized { get; private set; }
        [SerializeField] float ySmoothSpeed = 20f;

        private Vector3 _position;
        private Quaternion _rotation;
        private Vector3 _smoothPos;
        private Quaternion smoothRot;
        private float _smoothY;
        private bool _isInitializedY;

        private void Start()
        {
            WheelCollider = GetComponent<WheelCollider>();
        }

        private void Update()
        {
            WheelCollider.GetWorldPose(out _position, out _rotation);

            if (!_isInitializedY)
            {
                _smoothY = _position.y;
                _isInitializedY = true;
            }

            _smoothY = Mathf.Lerp(
                _smoothY,
                _position.y,
                Time.deltaTime * ySmoothSpeed
            );

            var finalPos = new Vector3(
                _position.x,
                _smoothY,
                _position.z
            );

            wheelModel.SetPositionAndRotation(finalPos, _rotation);
        }
    }
}