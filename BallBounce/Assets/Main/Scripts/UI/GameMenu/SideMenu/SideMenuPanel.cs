using System;
using Main.Scripts.GameLogic.GameFlow;
using Main.Scripts.UI.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.GameMenu.SideMenu
{
    public class SideMenuPanel : UIPanel
    {
        [SerializeField]
        private Button _settingsButton;

        [SerializeField]
        private Button _restartButton;

        private IGameFlowProvider _gameFlowProvider;

        public event Action OnSettingsClick;

        [Inject]
        public void Construct(IGameFlowProvider gameFlowProvider)
        {
            _gameFlowProvider = gameFlowProvider;
        }

        private void Start()
        {
            _settingsButton.onClick.AddListener(OnSettingsButtonClick);
            _restartButton.onClick.AddListener(OnRestartButtonClick);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
            _restartButton.onClick.RemoveListener(OnRestartButtonClick);
        }

        private void OnSettingsButtonClick() => OnSettingsClick?.Invoke();

        private void OnRestartButtonClick()
        {
            _gameFlowProvider.RespawnBalls();
        }
    }
}