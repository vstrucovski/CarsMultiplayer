using PurrNet;
using Reflex.Core;
using Reflex.Enums;
using Scripts.Gameplay.Levels;
using Scripts.Infrastructure.Services;
using Scripts.UI;
using UnityEngine;
using Resolution = Reflex.Enums.Resolution;

namespace Scripts.Infrastructure
{
    public class GameInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private UIManager ui;
        [SerializeField] private NetworkManager networkManager;

        public void InstallBindings(ContainerBuilder container)
        {
            container.RegisterType(typeof(InputModeSwitcher), Lifetime.Singleton, Resolution.Eager);
            container.RegisterValue(ui);
            container.RegisterType(typeof(LevelsManager), Lifetime.Singleton, Resolution.Lazy);
            BindNetwork(container);
        }

        private void BindNetwork(ContainerBuilder container)
        {
            container.RegisterValue(networkManager, new[] {typeof(INetworkManager), 
                typeof(NetworkManager)}); //FIXME remove later, when found to register spawned object to network
            container.RegisterValue(networkManager);
        }
    }
}