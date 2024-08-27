using System.Collections.Generic;
using Main.Scripts.Configs.Balls;
using Main.Scripts.Configs.Core;
using UnityEngine;

namespace Main.Scripts.Configs
{
    public class BallProgressionConfig : ScriptableConfig
    {
        [SerializeField]
        private List<BallConfig> _balls;

        [SerializeField, Min(0)]
        [Tooltip(
            "Определяет уровень нового шарика, вычитая это значение из максимального уровня существующих шариков.")]
        private int _creationMinLevelDelta = 3;

        [SerializeField]
        private MoneyProgressionConfig _moneyPerBallHitConfig;

        [SerializeField]
        private MoneyProgressionConfig _ballCostConfig;

        [SerializeField]
        private MoneyProgressionConfig _mergeCostConfig;

        public BallConfig GetBallsConfig(int configId)
        {
            if (configId < _balls.Count)
                return _balls[configId];

            return _balls[0];
        }

        public int CreationMinLevelDelta => _creationMinLevelDelta;
        public MoneyProgressionConfig MoneyPerBallHitConfig => _moneyPerBallHitConfig;
        public MoneyProgressionConfig BallCostConfig => _ballCostConfig;
        public MoneyProgressionConfig MergeCostConfig => _mergeCostConfig;

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.BALL_CATEGORY;
#endif
    }
}