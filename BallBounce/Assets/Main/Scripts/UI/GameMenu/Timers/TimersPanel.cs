using System.Collections.Generic;
using System.Linq;
using Main.Scripts.Configs.Levels;
using Main.Scripts.Core.Enums;
using Main.Scripts.GameLogic.GameFlow;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.UI.Base;
using UnityEngine;
using Zenject;

namespace Main.Scripts.UI.GameMenu.Timers
{
    public class TimersPanel : UIPanel
    {
        [SerializeField]
        private List<Timer> _timers = new List<Timer>();

        private IGameFlowProvider _gameFlowProvider;

        [Inject]
        public void Construct(IGameLevelsConfigProvider gameLevelsConfigProvider, IGameFlowProvider gameFlowProvider)
        {
            _gameFlowProvider = gameFlowProvider;
        }

        public override void Show()
        {
            base.Show();
            foreach (Timer timer in _timers)
                timer.Hide(true);
        }

        public void CreateTimer(BoosterConfig boosterConfig)
        {
            Timer _timer = GetTimer(boosterConfig.BoosterType);
            if (_timer == null)
            {
                Debug.LogWarning("Timer prefab not found!");
                return;
            }

            switch (boosterConfig.BoosterType)
            {
                case BoosterType.ExtraEarning:
                    _gameFlowProvider.SetMoneyMultiplier(boosterConfig.Multiplier);
                    _timer.Initialize(boosterConfig.WorkTime, () => _gameFlowProvider.SetMoneyMultiplier(1));
                    _timer.Show();
                    break;
                case BoosterType.Null:
                case BoosterType.ExtraBalls:
                    break;
            }
        }

        private Timer GetTimer(BoosterType boosterType) =>
            _timers.FirstOrDefault(t => t.BoosterType == boosterType);
    }
}