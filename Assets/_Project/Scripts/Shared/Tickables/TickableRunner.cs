using Reflex.Attributes;
using UnityEngine;

namespace Scripts.Shared.Tickables
{
    public class TickableRunner : MonoBehaviour
    {
        private ITickableService _tickableService;

        [Inject]
        private void Constructor(ITickableService tickableService)
        {
            _tickableService = tickableService;
        }

        private void Update()
        {
            _tickableService.TickAll();
        }
    }
}