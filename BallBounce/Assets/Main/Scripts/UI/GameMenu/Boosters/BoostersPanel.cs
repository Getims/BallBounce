using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Scripts.Configs.Levels;
using Main.Scripts.Core.Enums;
using Main.Scripts.GameLogic.GameFlow;
using Main.Scripts.Infrastructure.Ads;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.UI.Base;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Main.Scripts.UI.GameMenu.Boosters
{
    public class BoostersPanel : UIPanel
    {
        [SerializeField]
        private List<BoostersButton> _boosterButtons;

        private IGameFlowProvider _gameFlowProvider;
        private BoostersSystemConfig _boostersSystemConfig;

        private Action<BoosterConfig> _createTimer;
        private List<BoostersButton> _availableBoosters;
        private Coroutine _spawnCO;

        private bool _isButtonsSetuped = false;
        private int _activateBoosterId = -1;
        private int _availableBoostersCount = -1;

        [Inject]
        public void Construct(IGameLevelsConfigProvider gameLevelsConfigProvider, IGameFlowProvider gameFlowProvider)
        {
            _gameFlowProvider = gameFlowProvider;
            _boostersSystemConfig = gameLevelsConfigProvider.Config.BoostersSystemConfig;
        }

        public void Initialize(Action<BoosterConfig> createTimer)
        {
            _createTimer = createTimer;

            if (!_isButtonsSetuped)
                SetupButtons();

            if (_isButtonsSetuped)
            {
                _activateBoosterId = 0;
                StartSpawnBoosterCoroutine();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_spawnCO != null)
                StopCoroutine(_spawnCO);
        }

        private void SetupButtons()
        {
            if (!_boostersSystemConfig.HasConfigs)
            {
                Hide();
                _isButtonsSetuped = false;
                return;
            }

            _availableBoosters = new List<BoostersButton>(_boosterButtons.Count);
            foreach (BoostersButton boosterButton in _boosterButtons)
            {
                SetupButton(boosterButton);
                if (boosterButton.IsInitialized)
                    _availableBoosters.Add(boosterButton);
                boosterButton.Hide();
            }

            Random rnd = new Random();
            _availableBoosters = _availableBoosters.OrderBy(x => rnd.Next()).ToList();
            _activateBoosterId = 0;

            _availableBoostersCount = _availableBoosters.Count;
            _isButtonsSetuped = _availableBoostersCount > 0;
        }

        private void SetupButton(BoostersButton boostersButton)
        {
            BoosterType boosterType = boostersButton.BoosterType;
            boostersButton.Initialize(_boostersSystemConfig.GetConfig(boosterType), _boostersSystemConfig.OnScreenTime,
                OnBoosterButtonClick);
        }

        private void StartSpawnBoosterCoroutine()
        {
            if (_spawnCO != null)
                StopCoroutine(_spawnCO);
            _spawnCO = StartCoroutine(SpawnBooster());
        }

        private IEnumerator SpawnBooster()
        {
            WaitForSeconds spawnDelay = new WaitForSeconds(_boostersSystemConfig.GetBoosterFrequency());
            yield return spawnDelay;

            TryToSpawnBooster();
            yield return null;

            StartSpawnBoosterCoroutine();
        }

        private void TryToSpawnBooster()
        {
            for (int i = _activateBoosterId; i < _availableBoostersCount; i++)
            {
                if (_availableBoosters[_activateBoosterId].IsShowed)
                    continue;

                _availableBoosters[_activateBoosterId].Show();
                break;
            }

            _activateBoosterId++;
            if (_activateBoosterId >= _availableBoostersCount)
                _activateBoosterId = 0;
        }

        private void OnBoosterButtonClick(BoostersButton boosterButton)
        {
            StartSpawnBoosterCoroutine();
            AdsManager.ShowRewarded(TryToUseBooster(boosterButton));
        }

        private Action<bool> TryToUseBooster(BoostersButton boosterButton)
        {
            void UseBooster(bool canApply)
            {
                if (!canApply)
                    return;

                switch (boosterButton.BoosterType)
                {
                    case BoosterType.ExtraBalls:
                        _gameFlowProvider.SpawnBall(boosterButton.BoosterConfig.Multiplier);
                        break;
                    case BoosterType.ExtraEarning:
                        _createTimer?.Invoke(boosterButton.BoosterConfig);
                        break;
                }

                if (_boostersSystemConfig.UseRandomBoosters)
                    SetupButton(boosterButton);
            }

            return UseBooster;
        }
    }
}