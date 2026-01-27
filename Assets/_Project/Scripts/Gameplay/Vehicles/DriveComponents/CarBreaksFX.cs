using UnityEngine;

namespace Scripts.Gameplay.Vehicles.DriveComponents
{
    public class CarBreaksFX : MonoBehaviour
    {
        [SerializeField] private CarControl carControl;
        [SerializeField] private CarLights lights;

        private void Update()
        {
            var isShowLight = carControl.IsBreaking || carControl.ReverseAccelerate;
            lights.ShowBack(isShowLight);
        }
    }
}