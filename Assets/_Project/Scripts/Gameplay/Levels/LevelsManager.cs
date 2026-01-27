using System;
using System.Threading;
using System.Threading.Tasks;
using PurrNet;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Scripts.Gameplay.Levels
{
    public class LevelsManager
    {
        private readonly LevelsPlaylist _playlist;
        private readonly NetworkManager _networkManager;
        private NetworkIdentity _spawnedLevel;
        private AsyncOperationHandle<GameObject> _currentHandle;
        private CancellationTokenSource _cts;

        public NetworkIdentity CurrentLevel => _spawnedLevel;
        public bool IsLevelSpawned => _spawnedLevel != null;

        public LevelsManager(LevelsPlaylist playlist, NetworkManager networkManager)
        {
            _playlist = playlist;
            _networkManager = networkManager;
        }

        public async void SpawnLevel(int index)
        {
            // if (_networkManager.isClient)
            // {
            //     Debug.LogWarning("Only server can spawn levels");
            //     return;
            // }

            try
            {
                await SpawnLevelAsync(index);
            }
            catch (Exception _)
            {
                Debug.LogError($"failed to spawn level ind: {index}");
            }
        }

        public async Task SpawnLevelAsync(int index)
        {
            if (_spawnedLevel != null)
            {
                Debug.LogWarning("level already spawned");
                return;
            }

            if (index < 0 || index >= _playlist.List.Count)
            {
                Debug.LogError($"invalid level index: {index}");
                return;
            }

            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            var levelReference = _playlist.List[index];
            _currentHandle = Addressables.InstantiateAsync(levelReference);
            await _currentHandle.Task;

            if (_cts.Token.IsCancellationRequested)
                return;

            if (_currentHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"failed to load level at index {index}");
                return;
            }

            var levelGO = _currentHandle.Result;
            _spawnedLevel = levelGO.GetComponent<NetworkIdentity>();

            if (_spawnedLevel == null)
            {
                Debug.LogError("level prefab must have NetworkIdentity");
                Addressables.ReleaseInstance(_currentHandle);
                return;
            }
            
            _networkManager.Spawn(levelGO);
            
            await Task.Yield();
            
            var childIdentities = levelGO.GetComponentsInChildren<NetworkIdentity>(true);
            foreach (var childIdentity in childIdentities)
            {
                if (childIdentity != _spawnedLevel && !childIdentity.isSpawned)
                {
                    _networkManager.Spawn(childIdentity.gameObject);
                }
            }
            
            Debug.Log($"Level spawned with {childIdentities.Length} network objects");
        }

        public void DespawnCurrentLevel()
        {
            _cts?.Cancel();

            if (_spawnedLevel != null)
            {
                _spawnedLevel.Despawn();
                _spawnedLevel = null;
            }

            if (_currentHandle.IsValid())
            {
                Addressables.ReleaseInstance(_currentHandle);
            }
        }
    }
}