using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Levels
{
    [Serializable]
    public class PrizeSystemConfig
    {
        [SerializeField, Min(1)]
        private int _spawnFrequency = 10;

        [SerializeField]
        private int _needBallsHit = 30;

        [SerializeField, Min(0)]
        private int _reward = 100;

        [SerializeField, Range(0, 100)]
        [InfoBox("Процент награды от прогесса уровня.")]
        private int _levelPercentReward = 0;

        public int SpawnFrequency => _spawnFrequency;
        public int NeedBallsHit => _needBallsHit;
        public int Reward => _reward;
        public int LevelPercentReward => _levelPercentReward;
    }
}