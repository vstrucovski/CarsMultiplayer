using Reflex.Attributes;
using Scripts.UI;
using UnityEngine;

namespace Scripts.Gameplay.Character
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private float maxDistance;
        [SerializeField] private Vector3 offset = Vector3.zero;
        [SerializeField] private LayerMask physLayer;

        private IInteractable _interactable;
        private UIManager _uiManager;
        private float _detectionInterval = 0.2f;
        private float _nextDetectionTime;
        
        [Inject]
        private void Constructor(UIManager uiManager)
        {
            _uiManager = uiManager;
        }

        public void Tick()
        {
            if (Time.time >= _nextDetectionTime)
            {
                DetectInteractable();
                _nextDetectionTime = Time.time + _detectionInterval;
            }

            UpdateUi();
        }

        private void UpdateUi()
        {
            if (_uiManager == null) return;
            _uiManager.CharacterHUD.ShowInteractHint(_interactable != null);
        }

        public void Interact()
        {
            if (_interactable == null) return;
            _interactable.Interact(gameObject);
        }

        private void DetectInteractable()
        {
            var ray = new Ray(transform.position + offset, transform.forward);
            if (Physics.Raycast(ray, out var hit, maxDistance, physLayer))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    _interactable = interactable;
                }
                else
                {
                    _interactable = null;
                }
            }
            else
            {
                _interactable = null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _interactable == null ? Color.green : Color.red;
            Gizmos.DrawRay(transform.position + offset, transform.forward * maxDistance);
        }
    }
}