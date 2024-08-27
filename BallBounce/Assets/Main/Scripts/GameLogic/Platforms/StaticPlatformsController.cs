using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.GameLogic.Platforms
{
    public class StaticPlatformsController : MonoBehaviour
    {
        [SerializeField]
        private List<JumpPlatform> _platforms;

        private int _showedPlatformsCount;

        public bool CanShowPlatform => _showedPlatformsCount < _platforms.Count;

        public void HideAllPlatforms()
        {
            _showedPlatformsCount = 0;
            foreach (JumpPlatform platform in _platforms)
                platform.Hide();
        }

        public bool ShowPlatform()
        {
            if (!CanShowPlatform)
                return false;

            _platforms[_showedPlatformsCount].Show();
            _showedPlatformsCount++;
            return true;
        }

        [Button]
        public void CollectPlatforms()
        {
#if UNITY_EDITOR
            JumpPlatform[] jumpPlatforms = GetComponentsInChildren<JumpPlatform>();
            _platforms.Clear();
            _platforms.AddRange(jumpPlatforms);
#endif
        }
    }
}