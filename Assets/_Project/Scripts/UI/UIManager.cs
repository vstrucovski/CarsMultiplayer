using UnityEngine;

namespace Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private CarHud carHUD;

        [SerializeField] private CharacterHUD characterHUD;
        // [SerializeField] private UIDocument mainMenu;

        public CarHud CarHud => carHUD;

        public CharacterHUD CharacterHUD => characterHUD;
        // public UIDocument MainMenu => mainMenu;

        public void Init()
        {
            carHUD.Hide(true);
            characterHUD.Hide(true);
        }

        public void ShowCharacter(bool isShow)
        {
            if (isShow)
            {
                characterHUD.Show();
            }
            else
            {
                characterHUD.Hide();
            }
        }

        public void ShowVehicle(bool isShow)
        {
            if (isShow)
            {
                carHUD.Show();
            }
            else
            {
                carHUD.Hide();
            }
        }
    }
}