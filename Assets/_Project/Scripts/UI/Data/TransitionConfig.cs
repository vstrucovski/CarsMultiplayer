using System;
using UnityEngine;

namespace Scripts.UI.Data
{
    [Serializable]
    public sealed class TransitionConfig
    {
        [field: SerializeField] public float viewShowSec { get; private set; } = 0.35f;
        [field: SerializeField] public float viewHideSec { get; private set; } = 0.15f;
    }
}