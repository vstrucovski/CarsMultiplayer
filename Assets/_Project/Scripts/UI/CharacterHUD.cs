using Scripts.UI.Shared;
using UnityEngine.UIElements;

namespace Scripts.UI
{
    public class CharacterHUD : UIView
    {
        private VisualElement _interactHintView;

        public void ShowInteractHint(bool isVisible)
        {
            _interactHintView ??= root.Q("Interact");
            _interactHintView.visible = isVisible;
        }
    }
}