using System;
using System.Collections.Generic;
using Main.Scripts.GameLogic.GameFlow;
using UnityEngine;

namespace Main.Scripts.GameLogic.Platforms
{
    public class DynamicPlatformsController : MonoBehaviour
    {
        [SerializeField]
        private DynamicPlatform _platformPrefab;

        [SerializeField]
        private Transform _container;

        [SerializeField]
        private Transform _spawnPoint;

        [SerializeField]
        private GameZone _flexZone;

        private List<DynamicPlatform> _platforms = new List<DynamicPlatform>();
        private int _showedPlatformsCount;
        private Action<int, Vector3, bool> _savePlatformPosition;

        public void Initialize(Action<int, Vector3, bool> savePlatformPosition)
        {
            _savePlatformPosition = savePlatformPosition;
        }

        public void HideAllPlatforms()
        {
            foreach (DynamicPlatform platform in _platforms)
                Destroy(platform.gameObject);

            _showedPlatformsCount = 0;
            _platforms.Clear();
        }

        public void ShowPlatform(Vector3 position, bool zeroPosition)
        {
            bool needTutorial = false;
            if (zeroPosition)
            {
                position = _spawnPoint.position;
                needTutorial = true;
            }

            DynamicPlatform platform = Instantiate(_platformPrefab, position, Quaternion.identity, _container);
            platform.Initialize(_flexZone, _showedPlatformsCount, OnChangePlatformPosition, needTutorial);
            platform.Hide();
            platform.Show();

            _platforms.Add(platform);
            _showedPlatformsCount++;
        }

        private void OnChangePlatformPosition(int platformId, Vector3 position) =>
            _savePlatformPosition?.Invoke(platformId, position, false);
    }
}