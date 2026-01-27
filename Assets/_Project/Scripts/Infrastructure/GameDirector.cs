using PurrNet;
using Reflex.Attributes;
using Scripts.Gameplay.CameraFeature;
using Scripts.Gameplay.Levels;
using Scripts.Infrastructure.Services;
using Scripts.Shared;
using Scripts.UI;
using UnityEngine;

namespace Scripts.Infrastructure
{
    public class GameDirector : NetworkIdentity
    {
        private UIManager _ui;
        private ICameraService _cameraService;
        private LevelsManager _levelsManager;
        private SimpleFSM<GameState> _fsm;
        private ITimeService _timeService;

        [Inject]
        private void Constructor(UIManager ui,
            ICameraService cameraService,
            ITimeService timeService,
            LevelsManager levelsManager)
        {
            _timeService = timeService;
            _levelsManager = levelsManager;
            _cameraService = cameraService;
            _ui = ui;
            SetupFSM();
        }

        private void Start()
        {
            _fsm.SwitchState(GameState.Prepare);
        }

        private void SetupFSM()
        {
            _fsm = new SimpleFSM<GameState>();
            _fsm.AddState(GameState.Prepare, PrepareGame);
            _fsm.AddState(GameState.Gameplay, EnterGameplay, ExitGameplay);
            _fsm.AddState(GameState.Paused, EnterPause, ExitPause);
        }

        protected override void OnSpawned(bool asServer)
        {
            base.OnSpawned(asServer);
            if (asServer)
            {
                Debug.Log("Server init");
                networkManager.onPlayerJoined += OnPlayerJoined;
            }
        }

        private void OnPlayerJoined(PlayerID player, bool isReconnect, bool asServer)
        {
            Debug.Log("OnPlayerJoined = " + player.id);
            if (!asServer)
                SyncLevelStateRPC(0);
        }

        [ObserversRpc]
        private void SyncLevelStateRPC(int levelIndex)
        {
            if (_levelsManager.CurrentLevel == null)
            {
                _levelsManager.SpawnLevel(levelIndex);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (networkManager != null)
                networkManager.onPlayerJoined -= OnPlayerJoined;
        }

        private void PrepareGame()
        {
            Application.targetFrameRate = 60;
            _ui.Init();

            _fsm.SwitchState(GameState.Gameplay);
        }

        private void EnterGameplay()
        {
            _ui.ShowCharacter(true);
            _cameraService.SwitchToCharacter();
        }

        private void ExitGameplay()
        {
        }

        private void EnterPause()
        {
            _timeService.Pause();
        }

        private void ExitPause()
        {
            _timeService.Unpause();
        }
    }
}