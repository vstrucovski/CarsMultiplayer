using System.Collections.Generic;

namespace Scripts.Shared.Tickables
{
    public class TickableService : ITickableService
    {
        private List<ITickable> _list = new();

        public void Register(ITickable tickable)
        {
            _list.Add(tickable);
        }

        public void UnRegister(ITickable tickable)
        {
            _list.Remove(tickable);
        }

        public void TickAll()
        {
            for (var i = 0; i < _list.Count; i++)
            {
                _list[i].Tick();
            }
        }
    }
}