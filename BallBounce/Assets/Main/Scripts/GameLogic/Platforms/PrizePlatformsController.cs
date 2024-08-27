using System.Collections.Generic;
using Main.Scripts.Configs.Levels;
using Main.Scripts.Data.Services;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.Infrastructure.Providers.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Main.Scripts.GameLogic.Platforms
{
    public class PrizePlatformsController : MonoBehaviour
    {
        [SerializeField]
        private PrizePlatform _prizePlatform;

        [SerializeField]
        private List<Transform> _prizePoints;

        private PrizeSystemConfig _prizeSystemConfig;
        private GlobalEventProvider _globalEventProvider;
        private IProgressDataService _progressDataService;

        [Inject]
        public void Construct(IGameLevelsConfigProvider gameLevelsConfigProvider,
            GlobalEventProvider globalEventProvider, IProgressDataService progressDataService)
        {
            _progressDataService = progressDataService;
            _globalEventProvider = globalEventProvider;
            _prizeSystemConfig = gameLevelsConfigProvider.Config.PrizeSystemConfig;
        }

        public void Initialize() => StartTimer();

        public void HidePrize() => _prizePlatform.Hide();

        private void OnDestroy() => CancelInvoke(nameof(ShowPrize));

        private void StartTimer()
        {
            CancelInvoke(nameof(ShowPrize));
            Invoke(nameof(ShowPrize), _prizeSystemConfig.SpawnFrequency);
        }

        [Button]
        private void ShowPrize()
        {
            Vector3 prizePoint = GetPoint();
            _prizePlatform.Hide(true);
            _prizePlatform.transform.position = prizePoint;
            _prizePlatform.Initialize(_prizeSystemConfig.NeedBallsHit, GivePrizeReward);
            _prizePlatform.Show();
        }

        private void GivePrizeReward()
        {
            int reward = (int) (_progressDataService.LevelProgress * _prizeSystemConfig.LevelPercentReward) / 100;
            reward += _prizeSystemConfig.Reward;

            _globalEventProvider.Invoke<GivePrizeRewardEvent, int>(reward);
            _prizePlatform.Hide();
            StartTimer();
        }

        private Vector3 GetPoint()
        {
            if (_prizePoints.Count > 0)
                return _prizePoints[Random.Range(0, _prizePoints.Count)].position;

            return transform.position;
        }
    }
}