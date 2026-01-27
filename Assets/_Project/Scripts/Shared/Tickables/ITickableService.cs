namespace Scripts.Shared.Tickables
{
    public interface ITickableService
    {
        void UnRegister(ITickable tickable);
        void Register(ITickable tickable);
        void TickAll();
    }
}