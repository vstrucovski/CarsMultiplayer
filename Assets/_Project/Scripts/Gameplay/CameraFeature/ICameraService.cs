using UnityEngine;

namespace Scripts.Gameplay.CameraFeature
{
    public interface ICameraService
    {
        Camera MainCamera { get; }
        void SwitchToCharacter();
        void SwitchToVehicle(Transform carActorTransform);
        void SetTarget(Transform transform);
        void Shake(float power = 0.35f);
    }
}