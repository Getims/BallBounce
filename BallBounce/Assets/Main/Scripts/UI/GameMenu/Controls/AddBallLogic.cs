using Main.Scripts.Configs;
using Main.Scripts.Data.Services;

namespace Main.Scripts.UI.GameMenu.Controls
{
    public class AddBallLogic : ControlLogic
    {
        private readonly IPlayerDataService _playerDataService;
        private readonly BallProgressionConfig _ballProgressionConfig;
        private readonly IProgressDataService _progressDataService;

        public AddBallLogic(IPlayerDataService playerDataService, BallProgressionConfig ballProgressionConfig,
            IProgressDataService progressDataService)
        {
            _progressDataService = progressDataService;
            _playerDataService = playerDataService;
            _ballProgressionConfig = ballProgressionConfig;
        }

        protected override void UpdatePrice()
        {
            _cost = _ballProgressionConfig.BallCostConfig.Get(_progressDataService.BallSpawnCount);
            _setPrice?.Invoke(_cost);
        }

        protected override void UpdateAds()
        {
            _canShowAds = _progressDataService.BallSpawnCount > 5;
        }

        protected override bool IsEnoughMoney() =>
            _cost <= _playerDataService.Money;

        protected override bool IsConditionComplete() =>
            true;
    }
}