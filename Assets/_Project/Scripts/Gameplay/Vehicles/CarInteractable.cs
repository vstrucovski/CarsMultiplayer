using Reflex.Attributes;
using Scripts.Gameplay.Character;
using Scripts.Infrastructure.Services;
using UnityEngine;

namespace Scripts.Gameplay.Vehicles
{
    public class CarInteractable : MonoBehaviour, IInteractable
    {
        private InputModeSwitcher _inputModeSwitcher;
        [SerializeField] private CarActor _carActor;
        
        [Inject]
        private void Constructor(InputModeSwitcher inputModeSwitcher)
        {
            _inputModeSwitcher = inputModeSwitcher;
        }

        public void Interact(GameObject other)
        {
            if (!other.TryGetComponent(out CharacterActor characterActor)) return;
            _inputModeSwitcher.EnterCar(characterActor, _carActor);
        }
    }
}