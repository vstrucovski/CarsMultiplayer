using Scripts.UI.Data;
using UnityEngine;

namespace Scripts.UI
{
    [CreateAssetMenu(menuName = "Game/Config/UI")]
    public class UiConfig : ScriptableObject
    {
        [field: SerializeField] public TransitionConfig transition { get; private set; }
    }
}