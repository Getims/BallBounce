using System;
using Main.Scripts.Configs.Levels;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic.Sound;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.UI.Base;
using Main.Scripts.UI.Common;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.GameOver
{
    public class LevelCompletePanel : UIPanel
    {
        [SerializeField]
        private Button _switchButton;

        [SerializeField]
        private float _fadeOnHideDelay = 0.25f;

        [SerializeField]
        private SunRays _sunRays;

        [SerializeField]
        private Image _levelPreview;

        private ISoundService _soundService;
        private IProgressDataService _progressDataService;
        private IGameLevelsConfigProvider _gameLevelsConfigProvider;

        public event Action OnSwitchButtonClickEvent;

        [Inject]
        public void Construct(ISoundService soundService, IProgressDataService progressDataService,
            IGameLevelsConfigProvider gameLevelsConfigProvider)
        {
            _gameLevelsConfigProvider = gameLevelsConfigProvider;
            _progressDataService = progressDataService;
            _soundService = soundService;
        }

        public override void Show()
        {
            base.Show();
            _soundService.PlayWinSound();
            _sunRays.PlayAnimation();
            SetupLevelPreview();
        }

        public override void Hide()
        {
            _sunRays.StopAnimation();
            base.Hide(_fadeOnHideDelay);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _switchButton.onClick.RemoveListener(OnSwitchButtonClick);
        }

        protected virtual void Start() =>
            _switchButton.onClick.AddListener(OnSwitchButtonClick);

        private void SetupLevelPreview()
        {
            int currentLevel = _progressDataService.CurrentLevel;
            LevelConfig levelConfig = _gameLevelsConfigProvider.GetLevel(currentLevel + 1);
            _levelPreview.sprite = levelConfig.Preview;
        }

        private void OnSwitchButtonClick()
        {
            OnSwitchButtonClickEvent?.Invoke();
            Hide();
        }
    }
}