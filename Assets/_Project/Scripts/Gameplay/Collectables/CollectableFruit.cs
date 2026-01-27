using System.Collections;
using DG.Tweening;
using PurrNet;
using UnityEngine;

namespace Scripts.Gameplay.Collectables
{
    [SelectionBase]
    public class CollectableFruit : NetworkBehaviour
    {
        [SerializeField] private Transform model;
        [SerializeField] private int value = 1;
        [SerializeField] private float cooldownSec = 3f;

        private bool _isReadyToTrigger = true;

        private void OnTriggerEnter(Collider other)
        {
            if (!isServer || !_isReadyToTrigger) return;

            if (!isSpawned)
            {
                Debug.LogWarning($"{name} not spawned yet, ignoring trigger");
                return;
            }

            if (other.TryGetComponent(out ICollector _))
            {
                Collect();
            }
        }

        private void Collect()
        {
            _isReadyToTrigger = false;
            CollectFruitRPC();
            StartCoroutine(Cooldown());
        }

        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(cooldownSec);
            _isReadyToTrigger = true;
            RespawnFruitRPC();
        }

        [ObserversRpc]
        private void CollectFruitRPC()
        {
            model.DOKill();
            model.DOScale(0f, 0.35f)
                .SetEase(Ease.InOutExpo)
                .SetLink(gameObject);
        }

        [ObserversRpc]
        private void RespawnFruitRPC()
        {
            model.DOKill();
            model.DOScale(1f, 0.55f)
                .SetEase(Ease.OutElastic)
                .SetLink(gameObject);
        }
    }
}