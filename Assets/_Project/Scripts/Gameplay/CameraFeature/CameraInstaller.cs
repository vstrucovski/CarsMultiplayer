using Reflex.Core;
using Reflex.Enums;
using Unity.Cinemachine;
using UnityEngine;
using Resolution = Reflex.Enums.Resolution;

namespace Scripts.Gameplay.CameraFeature
{
    public class CameraInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineCamera characterVCam;
        [SerializeField] private CinemachineCamera vehicleVCam;

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(typeof(CameraService), Lifetime.Singleton, Resolution.Lazy);
            var service = new CameraService(mainCamera, characterVCam, vehicleVCam);
            containerBuilder.RegisterValue(service, new[] {typeof(ICameraService)});
        }
    }
}