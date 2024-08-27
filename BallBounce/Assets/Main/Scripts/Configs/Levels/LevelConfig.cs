using Main.Scripts.Configs.Core;
using Main.Scripts.GameLogic.Levels;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Levels
{
    public class LevelConfig : ScriptableConfig
    {
        [SerializeField]
        private float _targetMoney;

        [SerializeField]
        private LevelController _levelPrefab;

        [SerializeField, PreviewField(ObjectFieldAlignment.Left, Height = 80)]
        private Sprite _preview;

        public LevelController LevelPrefab => _levelPrefab;
        public float TargetMoney => _targetMoney;
        public Sprite Preview => _preview;

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.LEVEL_CATEGORY;
#endif
    }
}