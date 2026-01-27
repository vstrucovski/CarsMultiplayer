using Reflex.Core;
using Reflex.Enums;
using Scripts.Gameplay.Levels;
using Scripts.Infrastructure.Services;
using Scripts.Shared.Tickables;
using Scripts.UI;
using UnityEngine;
using Resolution = Reflex.Enums.Resolution;

namespace Scripts.Infrastructure
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private UiConfig uiConfig;
        [SerializeField] private LevelsPlaylist levelsPlaylist;

        public void InstallBindings(ContainerBuilder container)
        {
            container.RegisterValue(uiConfig);
            container.RegisterValue(levelsPlaylist);
            container.RegisterType(typeof(TimeService), new[] {typeof(ITimeService)}, Lifetime.Singleton,
                Resolution.Lazy);
            container.RegisterType(typeof(TickableService), new[] {typeof(ITickableService)}, Lifetime.Singleton,
                Resolution.Lazy);
        }
    }
}