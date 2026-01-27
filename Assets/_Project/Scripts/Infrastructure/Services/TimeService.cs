using UnityEngine;

namespace Scripts.Infrastructure.Services
{
    public class TimeService : ITimeService
    {
        public void Pause()
        {
            Time.timeScale = 0f;
        }

        public void Unpause()
        {
            Time.timeScale = 1f;
        }
    }
}