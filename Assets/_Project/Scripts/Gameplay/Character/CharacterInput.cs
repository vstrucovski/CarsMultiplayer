using UnityEngine;

namespace Scripts.Gameplay.Character
{
    public class CharacterInput : MonoBehaviour
    {
        private InputSystem_Actions inputMap;
        private bool isJumped;
        private Vector2 moveInput;
        private bool isRun;
        private bool isInteracted;

        public bool IsJumped => isJumped;
        public Vector2 MoveInput => moveInput;
        public bool IsRun => isRun;
        public bool IsInteracted => isInteracted;
        public InputSystem_Actions InputMap => inputMap;

        private void Awake()
        {
            inputMap = new InputSystem_Actions();
        }

        public void ActivateCharacter(bool isActive)
        {
            if (isActive)
            {
                InputMap.Player.Enable();
            }
            else
            {
                InputMap.Player.Disable();
            }
        }

        public void ActivateVehicle(bool isActive)
        {
            if (isActive)
            {
                InputMap.Car.Enable();
            }
            else
            {
                InputMap.Car.Disable();
            }
        }

        public void ReadValues()
        {
            if (inputMap == null)
            {
                return;
            }

            moveInput = InputMap.Player.Move.ReadValue<Vector2>();
            isJumped = InputMap.Player.Jump.WasPressedThisFrame();
            isRun = InputMap.Player.Run.IsPressed();
            isInteracted = InputMap.Player.Interact.WasPressedThisFrame();
        }

        private void OnDestroy()
        {
            InputMap.UI.Disable();
            InputMap.Car.Disable();
            InputMap.Player.Disable();
        }
    }
}