using UnityEngine;

namespace Main.Scripts.GameLogic.Platforms
{
    public abstract class Platform : MonoBehaviour
    {
        public abstract void BallHit();
        public abstract bool CanGiveMoney();
    }
}