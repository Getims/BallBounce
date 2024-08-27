using System;
using Main.Scripts.Data.Core;

namespace Main.Scripts.Data
{
    [Serializable]
    public class PlayerData : GameData
    {
        public bool IsSoundOn = true;
        public bool IsMusicOn = true;
        public int Money;
    }
}