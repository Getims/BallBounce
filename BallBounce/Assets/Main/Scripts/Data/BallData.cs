using System;
using UnityEngine;

namespace Main.Scripts.Data
{
    [Serializable]
    public class BallData
    {
        [SerializeField]
        public int BallLevel;

        public BallData(int ballLevel)
        {
            BallLevel = ballLevel;
        }
    }
}