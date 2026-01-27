using Reflex.Attributes;
using UnityEngine;

namespace Scripts.Shared.Tickables
{
    public abstract class TickableBehavior : MonoBehaviour, ITickable
    {
        private ITickableService _service;

        [Inject]
        private void Constructor(ITickableService service)
        {
            _service = service;
        }

        private void OnEnable()
        {
            if (_service != null)
                _service.Register(this);
        }

        private void OnDisable()
        {
            if (_service != null)
                _service.UnRegister(this);
        }

        public abstract void Tick();
    }
}