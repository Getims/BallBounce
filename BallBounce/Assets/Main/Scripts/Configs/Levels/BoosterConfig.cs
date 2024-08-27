using Main.Scripts.Configs.Core;
using Main.Scripts.Core.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Levels
{
    public class BoosterConfig : ScriptableConfig
    {
        [SerializeField]
        private BoosterType _boosterType = BoosterType.Null;

        [SerializeField, Min(1)]
        [ShowIf(nameof(IsBallsBooster))]
        private int _ballsCount = 1;

        [SerializeField, Min(1)]
        [ShowIf(nameof(IsExtraEarningBooster))]
        private int _earnMultiplier = 1;

        [SerializeField, Min(1)]
        [ShowIf(nameof(IsExtraEarningBooster))]
        private int _workTime = 1;

        public BoosterType BoosterType => _boosterType;
        public int WorkTime => _workTime;
        public int Multiplier => IsBallsBooster ? _ballsCount : _earnMultiplier;

        private bool IsBallsBooster => _boosterType == BoosterType.ExtraBalls;
        private bool IsExtraEarningBooster => _boosterType == BoosterType.ExtraEarning;

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.LEVEL_CATEGORY;
#endif
    }
}