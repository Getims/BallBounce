using Main.Scripts.Configs;
using Main.Scripts.Configs.Core;
using Main.Scripts.Data.Services;
using UnityEngine;

namespace Main.Scripts.UI.GameMenu.Controls
{
    public class AddDynamicPlatformLogic : ControlLogic
    {
        private readonly IPlayerDataService _playerDataService;
        private readonly MoneyProgressionConfig _platformsCostProgression;
        private readonly BallProgressionConfig _ballProgressionConfig;
        private readonly IProgressDataService _progressDataService;

        private float _lastAddTime = 0;
        private int _checkTime = 60;

        public AddDynamicPlatformLogic(IPlayerDataService playerDataService,
            MoneyProgressionConfig platformsCostProgression, IProgressDataService progressDataService)
        {
            _progressDataService = progressDataService;
            _playerDataService = playerDataService;
            _platformsCostProgression = platformsCostProgression;
            _lastAddTime = Time.time;
        }

        protected override void UpdatePrice()
        {
            _cost = _platformsCostProgression.Get(_progressDataService.DynamicPlatforms.Count);
            _setPrice?.Invoke(_cost);
        }

        protected override void UpdateAds()
        {
            int platformsCount = _progressDataService.DynamicPlatforms.Count;
            _canShowAds = platformsCount > 0
                          && Time.time > _lastAddTime + _checkTime * platformsCount;
        }

        protected override bool IsEnoughMoney() =>
            _cost <= _playerDataService.Money;

        protected override bool IsConditionComplete()
        {
            return true;
        }

        public override void OnClick()
        {
            base.OnClick();
            _canShowAds = false;
            _lastAddTime = Time.time;
        }

        public override void OnAdsClick()
        {
            base.OnAdsClick();
            _canShowAds = false;
            _lastAddTime = Time.time;
        }
    }
}