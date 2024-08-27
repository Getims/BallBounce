using System;
using Main.Scripts.UI.Base;
using Main.Scripts.UI.Common;
using Main.Scripts.UI.Common.FlyIcons;
using UnityEngine;

namespace Main.Scripts.UI.GameMenu
{
    public class TopGamePanel : UIPanel
    {
        [SerializeField]
        private LevelTracker _levelTracker;

        [SerializeField]
        private LevelProgressTracker _levelProgressTracker;

        [SerializeField]
        private MoneyCounter _moneyCounter;

        [SerializeField]
        private FlyIconsSpawner _flyIconsSpawner;

        private Coroutine _showMoneyCO;
        private int _coinsPerGameElement;

        public override void Show()
        {
            base.Show();

            _moneyCounter.UpdateInfo();
            _levelTracker.UpdateInfo();
            _levelProgressTracker.UpdateInfo();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_showMoneyCO != null)
                StopCoroutine(_showMoneyCO);
        }

        public void ShowMoneyReward(Vector3 moneyIconPosition, Action OnRewardComplete)
        {
            _flyIconsSpawner.StartAnimation(moneyIconPosition, _moneyCounter.IconPosition);
            OnRewardComplete?.Invoke();
        }
    }
}