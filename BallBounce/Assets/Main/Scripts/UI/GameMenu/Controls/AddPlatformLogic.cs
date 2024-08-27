using System;
using Main.Scripts.Configs;
using Main.Scripts.Configs.Core;
using Main.Scripts.Data.Services;

namespace Main.Scripts.UI.GameMenu.Controls
{
    public class AddPlatformLogic : ControlLogic
    {
        private readonly IPlayerDataService _playerDataService;
        private readonly MoneyProgressionConfig _platformsCostProgression;
        private readonly BallProgressionConfig _ballProgressionConfig;
        private readonly IProgressDataService _progressDataService;
        private readonly Func<bool> _canAddPad;

        public AddPlatformLogic(IPlayerDataService playerDataService, MoneyProgressionConfig platformsCostProgression,
            IProgressDataService progressDataService, Func<bool> canAddPad)
        {
            _canAddPad = canAddPad;
            _progressDataService = progressDataService;
            _playerDataService = playerDataService;
            _platformsCostProgression = platformsCostProgression;
        }

        protected override void UpdatePrice()
        {
            _cost = _platformsCostProgression.Get(_progressDataService.PlatformsCount);
            _setPrice?.Invoke(_cost);
        }

        protected override void UpdateAds()
        {
            _canShowAds = _progressDataService.PlatformsCount > 2;
        }

        protected override bool IsEnoughMoney() =>
            _cost <= _playerDataService.Money;

        protected override bool IsConditionComplete()
        {
            return _canAddPad();
        }
    }
}