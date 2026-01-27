using Scripts.Gameplay.CameraFeature;
using Scripts.Gameplay.Character;
using Scripts.Gameplay.Vehicles;
using Scripts.UI;

namespace Scripts.Infrastructure.Services
{
    public class InputModeSwitcher
    {
        private readonly ICameraService _cameraService;
        private readonly UIManager _ui;

        public InputModeSwitcher(
            ICameraService cameraService,
            UIManager ui)
        {
            _ui = ui;
            _cameraService = cameraService;
        }

        public void EnterCar(CharacterActor playerCharacter, CarActor carActor)
        {
            _ui.ShowVehicle(true);
            _ui.ShowCharacter(false);
            _ui.CarHud.Bind(carActor);
            _cameraService.SwitchToVehicle(carActor.transform);
            carActor.SetOwner(playerCharacter);
            playerCharacter.Input.ActivateCharacter(false);
            playerCharacter.Input.ActivateVehicle(true);
        }

        public void ExitCar(CharacterActor playerCharacter)
        {
            _cameraService.SwitchToCharacter();
            _ui.ShowVehicle(false);
            _ui.ShowCharacter(true);
            _ui.CarHud.Unbind();
            
            playerCharacter.Input.ActivateCharacter(true);
            playerCharacter.Input.ActivateVehicle(false);
        }
    }
}