using System;
using System.Collections.Generic;
using Main.Scripts.Data.Core;
using UnityEngine;

namespace Main.Scripts.Data
{
    [Serializable]
    public class ProgressData : GameData
    {
        [SerializeField]
        public int CurrentLevel = 0;

        [SerializeField]
        public float LevelProgress = 0;

        [SerializeField]
        public List<BallData> BallsData = new List<BallData>();

        [SerializeField]
        public int BallSpawnCount = 0;

        [SerializeField]
        public int BallMergeCount = 0;

        [SerializeField]
        public int PlatformsCount = 0;

        [SerializeField]
        public List<Vector3> DynamicPlatforms = new List<Vector3>();
    }
}