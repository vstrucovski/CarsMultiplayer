using Scripts.Gameplay.Vehicles;
using Scripts.UI.Shared;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.UI
{
    public class CarHud : UIView
    {
        private CarActor _car;
        private Label speedText;

        public void Bind(CarActor car)
        {
            speedText = root.Q<Label>("SpeedText");
            Unbind();
            _car = car;
            _car.OnSpeedChanged += UpdateSpeed;
        }

        public void Unbind()
        {
            if (_car != null)
                _car.OnSpeedChanged -= UpdateSpeed;
        }

        private void UpdateSpeed(float speed)
        {
            speedText.text = Mathf.RoundToInt(speed).ToString();
        }
    }
}