using DG.Tweening;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.UI.Shared
{
    public abstract class UIView : MonoBehaviour
    {
        protected VisualElement root;
        private UiConfig _uiConfig;

        [Inject]
        private void Constructor(UiConfig uiConfig)
        {
            _uiConfig = uiConfig;
        }

        private void Awake()
        {
            if (TryGetComponent(out UIDocument document))
            {
                Init(document);
            }
        }

        public virtual void Init(UIDocument document)
        {
            root = document.rootVisualElement;
        }

        public void Show(bool isFast = false)
        {
            ChangeOpacity(root, 1, isFast ? 0 : _uiConfig.transition.viewShowSec);
        }

        public void Hide(bool isFast = false)
        {
            ChangeOpacity(root, 0, isFast ? 0 : _uiConfig.transition.viewHideSec);
        }

        private void ChangeOpacity(VisualElement view, float value, float duration)
        {
            if (value == 0)
            {
                view.focusable = false;
                if (duration == 0)
                {
                    view.style.opacity = value;
                }
                else
                {
                    DOTween.To(() => view.style.opacity.value, x => view.style.opacity = x, value, duration)
                        .SetLink(gameObject);
                }
            }
            else
            {
                view.focusable = true;
                if (duration == 0)
                {
                    view.style.opacity = value;
                }
                else
                {
                    DOTween.To(() => view.style.opacity.value, x => view.style.opacity = x, value, duration)
                        .SetLink(gameObject);
                }
            }
        }
    }
}