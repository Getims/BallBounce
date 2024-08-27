using System;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic.GameFlow;
using Main.Scripts.Infrastructure.Ads;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.Infrastructure.Providers.Events;
using Main.Scripts.UI.Base;
using UnityEngine;
using Zenject;

namespace Main.Scripts.UI.GameMenu.Controls
{
    public class ControlPanel : UIPanel
    {
        [SerializeField]
        private ControlButton _addPlatformButton;

        [SerializeField]
        private ControlButton _addDynamicPlatformButton;

        [SerializeField]
        private ControlButton _addBallButton;

        [SerializeField]
        private ControlButton _mergeBallsButton;

        private IGameFlowProvider _gameFlowProvider;
        private IPlayerDataService _playerDataService;
        private GlobalEventProvider _globalEventProvider;
        private IBallProgressionConfigProvider _ballProgressionConfigProvider;
        private IProgressDataService _progressDataService;
        private IGameLevelsConfigProvider _gameLevelsConfigProvider;

        private bool _isInitialized = false;

        [Inject]
        public void Construct(IGameFlowProvider gameFlowProvider, IPlayerDataService playerDataService,
            GlobalEventProvider globalEventProvider, IBallProgressionConfigProvider ballProgressionConfigProvider,
            IProgressDataService progressDataService, IGameLevelsConfigProvider gameLevelsConfigProvider)
        {
            _gameLevelsConfigProvider = gameLevelsConfigProvider;
            _progressDataService = progressDataService;
            _ballProgressionConfigProvider = ballProgressionConfigProvider;
            _globalEventProvider = globalEventProvider;
            _playerDataService = playerDataService;
            _gameFlowProvider = gameFlowProvider;
            _globalEventProvider.AddListener<MoneyChangedEvent, int>(OnMoneyChange);
        }

        public void Initialize()
        {
            if (!_isInitialized)
            {
                SetupButtons();
                _isInitialized = true;
            }

            UpdateButtons();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _globalEventProvider.RemoveListener<MoneyChangedEvent, int>(OnMoneyChange);
        }

        private void SetupButtons()
        {
            AddBallLogic addBallLogic = new AddBallLogic(_playerDataService, _ballProgressionConfigProvider.Config,
                _progressDataService);
            _addBallButton.Initialize(addBallLogic, () => OnAddBall(), () => OnAddBall(true));

            MergeBallsLogic mergeBallsLogic = new MergeBallsLogic(_playerDataService,
                _ballProgressionConfigProvider.Config, _progressDataService, () => _gameFlowProvider.CanMergeBalls);
            _mergeBallsButton.Initialize(mergeBallsLogic, () => OnMergeBalls(), () => OnMergeBalls(true));

            AddPlatformLogic addPlatformLogic = new AddPlatformLogic(_playerDataService,
                _gameLevelsConfigProvider.Config.PlatformCostConfig, _progressDataService,
                () => _gameFlowProvider.CanSpawnStaticPlatforms);
            _addPlatformButton.Initialize(addPlatformLogic, () => OnAddPlatform(), () => OnAddPlatform(true));

            AddDynamicPlatformLogic addDynamicPlatformLogic = new AddDynamicPlatformLogic(_playerDataService,
                _gameLevelsConfigProvider.Config.DynamicPlatformCostConfig, _progressDataService);
            _addDynamicPlatformButton.Initialize(addDynamicPlatformLogic, () => OnAddDynamicPlatform(),
                () => OnAddDynamicPlatform(true));
        }

        private void UpdateButtons()
        {
            _addBallButton.UpdateState();
            _mergeBallsButton.UpdateState();
            _addPlatformButton.UpdateState();
            _addDynamicPlatformButton.UpdateState();
        }

        private void OnMoneyChange(int currentMoney) => UpdateButtons();

        private void OnAddBall(bool useAds = false)
        {
            if (useAds)
                OnAdsClick(() => _gameFlowProvider.SpawnBall());
            else
                OnMoneyClick(_addBallButton.Cost, () =>
                {
                    _gameFlowProvider.SpawnBall();
                    _progressDataService.IncreaseBallsSpawnCount();
                });
        }

        private void OnMergeBalls(bool useAds = false)
        {
            if (useAds)
                OnAdsClick(_gameFlowProvider.MergeBalls);
            else
                OnMoneyClick(_mergeBallsButton.Cost, () =>
                {
                    _gameFlowProvider.MergeBalls();
                    _progressDataService.IncreaseBallsMergeCount();
                });
        }

        private void OnAddPlatform(bool useAds = false)
        {
            if (useAds)
                OnAdsClick(_gameFlowProvider.SpawnStaticPlatform);
            else
                OnMoneyClick(_addPlatformButton.Cost, _gameFlowProvider.SpawnStaticPlatform);
        }

        private void OnAddDynamicPlatform(bool useAds = false)
        {
            if (useAds)
                OnAdsClick(_gameFlowProvider.SpawnDynamicPlatform);
            else
                OnMoneyClick(_addDynamicPlatformButton.Cost, _gameFlowProvider.SpawnDynamicPlatform);
        }

        private void OnMoneyClick(int cost, Action useBooster)
        {
            if (_playerDataService.Money < cost)
                return;

            _playerDataService.SpendMoney(cost);
            useBooster?.Invoke();
            UpdateButtons();
        }

        private void OnAdsClick(Action useBooster)
        {
            AdsManager.ShowRewarded((rewarded) =>
            {
                if (rewarded)
                    useBooster?.Invoke();
                UpdateButtons();
            });
        }
    }
}