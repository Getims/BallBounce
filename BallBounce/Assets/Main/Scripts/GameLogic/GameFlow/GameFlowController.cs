using Main.Scripts.Configs.Levels;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic.Balls;
using Main.Scripts.GameLogic.Levels;
using Main.Scripts.GameLogic.Money;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.Infrastructure.Providers.Events;
using UnityEngine;
using Zenject;

namespace Main.Scripts.GameLogic.GameFlow
{
    public class GameFlowController : MonoBehaviour, IGameFlowProvider
    {
        [SerializeField]
        private Transform _levelContainer;

        [SerializeField]
        private CameraController _cameraController;

        [SerializeField]
        private GameZone _gameZone;

        [SerializeField]
        private BallsController _ballsController;

        [SerializeField]
        private MoneyController _moneyController;

        private IGameLevelsConfigProvider _gameLevelsConfigProvider;
        private IProgressDataService _progressDataService;
        private GlobalEventProvider _globalEventProvider;

        private LevelConfig _levelConfigConfig;
        private LevelController _currentLevel;
        private bool _isLoaded = false;
        private bool _isGameComplete = false;

        public bool IsLoaded => _isLoaded;
        public bool CanMergeBalls => _ballsController.CanMergeBalls;
        public bool CanSpawnStaticPlatforms => _currentLevel.CanSpawnPlatform;

        [Inject]
        public void Construct(IGameLevelsConfigProvider gameLevelsConfigProvider,
            IProgressDataService progressDataService, GlobalEventProvider globalEventProvider)
        {
            _globalEventProvider = globalEventProvider;
            _progressDataService = progressDataService;
            _gameLevelsConfigProvider = gameLevelsConfigProvider;
        }

        public void Initialize()
        {
            int currentLevel = _progressDataService.CurrentLevel;
            _levelConfigConfig = _gameLevelsConfigProvider.GetLevel(currentLevel);

            if (_currentLevel != null)
                Destroy(_currentLevel.gameObject);

            _gameZone.Initialize();

            _currentLevel = Instantiate(_levelConfigConfig.LevelPrefab, _levelContainer);
            _currentLevel.Initialize();

            _moneyController.Initialize(_levelConfigConfig, OnLevelComplete);
            _ballsController.Initialize(_gameZone, _moneyController);
            _cameraController.Initialize(_gameZone, _ballsController.transform);
            _isGameComplete = false;
        }

        public void StartGame()
        {
            _ballsController.FirstBallSpawn();
        }

        public void SpawnBall(int count)
        {
            if (count <= 1)
                _ballsController.SpawnBall();
            else
                _ballsController.SpawnBalls(count);
        }

        public void MergeBalls() =>
            _ballsController.MergeBalls();

        public void RespawnBalls() =>
            _ballsController.RespawnBalls();

        public void SpawnStaticPlatform() =>
            _currentLevel.ShowStaticPlatform();

        public void SpawnDynamicPlatform() =>
            _currentLevel.ShowDynamicPlatform();

        public void SetMoneyMultiplier(float multiplier = 1) =>
            _moneyController.SetMultiplier(multiplier);

        private void Start() =>
            _isLoaded = true;

        private void OnLevelComplete()
        {
            if (_isGameComplete)
                return;

            _isGameComplete = true;
            _globalEventProvider?.Invoke<GameOverEvent, bool>(true);
        }
    }
}