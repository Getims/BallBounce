using Main.Scripts.Data.Services;
using Main.Scripts.Infrastructure.Providers.Events;
using Main.Scripts.UI.Base;
using UnityEngine;
using Zenject;

namespace Main.Scripts.UI.DebugPanel
{
    public class DebugUI : UIPanel
    {
        [SerializeField]
        private int _moneyToAdd;

        private IPlayerDataService _playerDataService;
        private GlobalEventProvider _globalEventProvider;
        private IProgressDataService _progressDataService;

        [Inject]
        public void Construct(IPlayerDataService playerDataService, IProgressDataService progressDataService,
            GlobalEventProvider globalEventProvider)
        {
            _progressDataService = progressDataService;
            _globalEventProvider = globalEventProvider;
            _playerDataService = playerDataService;
        }

        public void AddMoney()
        {
            _playerDataService.AddMoney(_moneyToAdd);

            float progress = _progressDataService.LevelProgress + _moneyToAdd;
            _progressDataService.SetLevelProgress(progress);
        }

        public void ResetData()
        {
            _playerDataService.SetMoney(0);
            _progressDataService.SetCurrentLevel(0);

            _globalEventProvider?.Invoke<GameOverEvent, bool>(false);
        }

        public void TryToShowDebug()
        {
            Show();
        }
    }
}