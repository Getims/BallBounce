using Main.Scripts.Configs.Levels;
using Main.Scripts.Data.Services;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.Infrastructure.Providers.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.Common
{
    public class LevelProgressTracker : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _valueTMP;

        [SerializeField]
        private Image _progressBar;

        private GlobalEventProvider _globalEventProvider;
        private IProgressDataService _progressDataService;
        private IGameLevelsConfigProvider _gameLevelsConfigProvider;
        private int _currentLevel = -1;
        private float _currentTarget = -1;

        [Inject]
        public void Construct(IProgressDataService progressDataService, GlobalEventProvider globalEventProvider,
            IGameLevelsConfigProvider gameLevelsConfigProvider)
        {
            _gameLevelsConfigProvider = gameLevelsConfigProvider;
            _progressDataService = progressDataService;
            _globalEventProvider = globalEventProvider;
            _globalEventProvider.AddListener<ProgressChangeEvent, float>(UpdateInfo);
        }

        public void UpdateInfo()
        {
            if (_progressDataService == null)
                return;

            SetupLevelInfo();
            UpdateInfo(_progressDataService.LevelProgress);
        }

        private void Start() =>
            UpdateInfo();

        private void OnDestroy() =>
            _globalEventProvider?.RemoveListener<ProgressChangeEvent, float>(UpdateInfo);

        private void SetupLevelInfo()
        {
            int levelId = _progressDataService.CurrentLevel;
            if (_currentLevel == -1 || _currentLevel != levelId)
            {
                _currentLevel = levelId;
                LevelConfig levelConfig = _gameLevelsConfigProvider.GetLevel(_currentLevel);
                _currentTarget = levelConfig.TargetMoney;
            }
        }

        private void UpdateInfo(float currentProgress)
        {
            UpdateText(currentProgress);
            UpdateProgressBar(currentProgress);
        }

        private void UpdateProgressBar(float currentProgress)
        {
            float value = 0;
            if (_currentTarget > 0)
                value = currentProgress / _currentTarget;

            _progressBar.fillAmount = value;
        }

        private void UpdateText(float currentProgress) =>
            _valueTMP.text = $"{currentProgress / 1000:F2}/{_currentTarget / 1000}K";
    }
}