using System;
using System.Collections.Generic;
using System.Linq;
using Main.Scripts.Core.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Scripts.Configs.Levels
{
    [Serializable]
    public class BoostersSystemConfig
    {
        [SerializeField]
        private List<BoosterConfig> _boostersConfigs = new List<BoosterConfig>();

        [SerializeField]
        private bool _useRandomBoosters = true;

        [SerializeField, Min(0)]
        [Tooltip("Сколько времени на экране висит кнопка бустера")]
        private int _onScreenTime = 120;

        [SerializeField, MinMaxSlider(1, 180)]
        private Vector2Int _newBoosterFrequency = new Vector2Int(60, 120);

        public int OnScreenTime => _onScreenTime;
        public bool HasConfigs => _boostersConfigs.Count > 0;
        public bool UseRandomBoosters => _useRandomBoosters;

        public BoosterConfig GetConfig(BoosterType boosterType)
        {
            if (_useRandomBoosters)
            {
                System.Random random = new System.Random();
                return _boostersConfigs
                    .Where(bc => bc.BoosterType == boosterType)
                    .OrderBy(bc => random.Next()) // Сортируем в случайном порядке
                    .FirstOrDefault();
            }

            return _boostersConfigs.FirstOrDefault(bc => bc.BoosterType == boosterType);
        }

        public int GetBoosterFrequency() =>
            Random.Range(_newBoosterFrequency.x, _newBoosterFrequency.y + 1);
    }
}