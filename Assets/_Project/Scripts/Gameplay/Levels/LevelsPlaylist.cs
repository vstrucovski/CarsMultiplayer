using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts.Gameplay.Levels
{
    [CreateAssetMenu(menuName = "Game/Config/Levels playlist")]
    public class LevelsPlaylist : ScriptableObject
    {
        [field: SerializeField] public List<AssetReferenceGameObject> List { get; private set; }
        
        [field: SerializeField] public GameObject level01 { get; private set; } 
    }
}