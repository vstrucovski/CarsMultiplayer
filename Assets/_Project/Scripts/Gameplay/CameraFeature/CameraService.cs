using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace Scripts.Gameplay.CameraFeature
{
    public class CameraService : ICameraService
    {
        private readonly Camera _mainCamera;
        private readonly CinemachineCamera _characterVCam;
        private readonly CinemachineCamera _vehicleVCam;
        private readonly CinemachineOrbitalFollow _carFollow;
        private readonly CinemachineOrbitalFollow _characterOrbit;

        public Camera MainCamera => _mainCamera;

        public CameraService(
            Camera mainCamera,
            CinemachineCamera characterVCam,
            CinemachineCamera vehicleVCam)
        {
            _mainCamera = mainCamera;
            _vehicleVCam = vehicleVCam;
            _characterVCam = characterVCam;

            _characterOrbit = _characterVCam.GetComponent<CinemachineOrbitalFollow>();
            _carFollow = _vehicleVCam.GetComponent<CinemachineOrbitalFollow>();
        }

        public void SetTarget(Transform transform)
        {
            _characterVCam.Target.TrackingTarget = transform;
            _characterVCam.Target.LookAtTarget = transform;
            _vehicleVCam.Target.TrackingTarget = transform;
            _vehicleVCam.Target.LookAtTarget = transform;
        }

        public void SwitchToCharacter()
        {
            _characterVCam.transform.rotation = _vehicleVCam.transform.rotation;
            _characterVCam.gameObject.SetActive(true);
            _vehicleVCam.gameObject.SetActive(false);
        }

        public void SwitchToVehicle(Transform target)
        {
            _carFollow.HorizontalAxis.Recentering.Enabled = true;
            _vehicleVCam.transform.rotation = _characterVCam.transform.rotation;
            _vehicleVCam.Target.TrackingTarget = target;
            _characterVCam.gameObject.SetActive(false);
            _vehicleVCam.gameObject.SetActive(true);
        }
        
        public void Shake(float power = 0.5f)
        {
            //TODO cache current active vcam and apply shake to it
            var old = _characterOrbit.TargetOffset;

            DOVirtual.Vector3(old, old + Vector3.up * power, 0.15f, value => { _characterOrbit.TargetOffset = value; })
                .SetEase(Ease.InBack)
                .SetLoops(2, LoopType.Yoyo);
        }
    }
}