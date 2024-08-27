using System;
using Main.Scripts.Configs;
using Main.Scripts.Data.Services;

namespace Main.Scripts.UI.GameMenu.Controls
{
    public class MergeBallsLogic : ControlLogic
    {
        private readonly IPlayerDataService _playerDataService;
        private readonly BallProgressionConfig _ballProgressionConfig;
        private readonly IProgressDataService _progressDataService;
        private readonly Func<bool> _canMerge;

        public MergeBallsLogic(IPlayerDataService playerDataService, BallProgressionConfig ballProgressionConfig,
            IProgressDataService progressDataService, Func<bool> canMerge)
        {
            _canMerge = canMerge;
            _progressDataService = progressDataService;
            _playerDataService = playerDataService;
            _ballProgressionConfig = ballProgressionConfig;
        }

        protected override void UpdatePrice()
        {
            _cost = _ballProgressionConfig.MergeCostConfig.Get(_progressDataService.BallMergeCount);
            _setPrice?.Invoke(_cost);
        }

        protected override void UpdateAds()
        {
            _canShowAds = _progressDataService.BallMergeCount > 5;
        }

        protected override bool IsEnoughMoney() =>
            _cost <= _playerDataService.Money;

        protected override bool IsConditionComplete()
        {
            return _canMerge();
        }
    }
}