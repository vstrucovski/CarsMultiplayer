using Scripts.Infrastructure.Services;
using Scripts.Shared.Tickables;
using UnityEngine;

namespace Scripts.Gameplay.Collectables
{
    public class Rotator : TickableBehavior
    {
        [SerializeField] private float speed;
        
        public override void Tick()
        {
            transform.Rotate(Vector3.up, speed);
        }
    }
}