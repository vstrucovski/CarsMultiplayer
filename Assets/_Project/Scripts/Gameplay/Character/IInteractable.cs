using UnityEngine;

namespace Scripts.Gameplay.Character
{
    public interface IInteractable
    {
        void Interact(GameObject other);
    }
}