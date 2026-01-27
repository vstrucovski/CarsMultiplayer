using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Scripts.Gameplay.Vehicles.DriveComponents
{
    public class CarLights : MonoBehaviour
    {
        [SerializeField] private List<Light> frontLights;
        [SerializeField] private float frontIntensivity;

        [Space]
        [SerializeField] private List<Light> backLights;

        public void ShowFront(bool isOn)
        {
            foreach (var item in frontLights)
            {
                item.DOKill();
                if (isOn)
                {
                    item.DOIntensity(frontIntensivity, 0.3f)
                        .SetEase(Ease.InOutBounce)
                        .SetLoops(2, LoopType.Restart)
                        .SetLink(gameObject);
                }
                else
                {
                    item.DOIntensity(0, 0.6f)
                        .SetLink(gameObject);
                }
            }
        }

        public void ShowBack(bool isOn)
        {
            foreach (var item in backLights)
            {
                item.gameObject.SetActive(isOn);
            }
        }
    }
}