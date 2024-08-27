using System.Collections.Generic;
using Main.Scripts.Configs.Core;
using Main.Scripts.Configs.Levels;
using UnityEngine;

namespace Main.Scripts.Configs
{
    public class GameLevelsConfig : ScriptableConfig
    {
        [SerializeField]
        private List<LevelConfig> _levelConfigs = new List<LevelConfig>();

        [SerializeField]
        private MoneyProgressionConfig _platformCostConfig;

        [SerializeField]
        private MoneyProgressionConfig _dynamicPlatformCostConfig;

        [SerializeField]
        private BoostersSystemConfig _boostersSystemConfig;

        [SerializeField]
        private PrizeSystemConfig _prizeSystemConfig;

        public IReadOnlyCollection<LevelConfig> LevelConfigs => _levelConfigs;
        public MoneyProgressionConfig PlatformCostConfig => _platformCostConfig;
        public MoneyProgressionConfig DynamicPlatformCostConfig => _dynamicPlatformCostConfig;
        public BoostersSystemConfig BoostersSystemConfig => _boostersSystemConfig;
        public PrizeSystemConfig PrizeSystemConfig => _prizeSystemConfig;

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.LEVEL_CATEGORY;
#endif
    }
}