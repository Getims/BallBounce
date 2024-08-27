using System.Collections.Generic;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic.GameFlow;
using Main.Scripts.GameLogic.Platforms;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Main.Scripts.GameLogic.Levels
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        private StaticPlatformsController _staticPlatformsController;

        [SerializeField]
        private DynamicPlatformsController _dynamicPlatformsController;

        [SerializeField]
        private PrizePlatformsController _prizePlatformsController;

        private bool _firstPlatformSpawned = false;
        private IProgressDataService _progressDataService;

        public bool CanSpawnPlatform => _staticPlatformsController.CanShowPlatform;

        [Inject]
        public void Construct(IProgressDataService progressDataService)
        {
            _progressDataService = progressDataService;
        }

        public void Initialize()
        {
            InitializeStaticPlatforms();
            InitializeDynamicPlatforms();
            InitializePrizeController();
        }

        [Button]
        public bool ShowStaticPlatform()
        {
            bool wasShown = _staticPlatformsController.ShowPlatform();
            if (wasShown)
                _progressDataService.IncreasePlatformsCount();

            return wasShown;
        }

        [Button]
        public void ShowDynamicPlatform()
        {
            _progressDataService.IncreaseDynamicPlatformsCount();
            _dynamicPlatformsController.ShowPlatform(Vector3.zero, true);
        }

        private void InitializeStaticPlatforms()
        {
            if (_firstPlatformSpawned)
                return;

            _staticPlatformsController.HideAllPlatforms();

            int dataPlatformsCount = _progressDataService.PlatformsCount;
            if (dataPlatformsCount == 0)
                ShowStaticPlatform();
            else
            {
                for (int i = 0; i < dataPlatformsCount; i++)
                    _staticPlatformsController.ShowPlatform();
            }

            _firstPlatformSpawned = true;
        }

        private void InitializeDynamicPlatforms()
        {
            _dynamicPlatformsController.Initialize(_progressDataService.SetDynamicPlatformPosition);
            _dynamicPlatformsController.HideAllPlatforms();

            IReadOnlyCollection<Vector3> dynamicPlatforms = new List<Vector3>(_progressDataService.DynamicPlatforms);
            if (dynamicPlatforms.Count > 0)
            {
                foreach (Vector3 platformPosition in dynamicPlatforms)
                    _dynamicPlatformsController.ShowPlatform(platformPosition, false);
            }
        }

        private void InitializePrizeController()
        {
            _prizePlatformsController.HidePrize();
            _prizePlatformsController.Initialize();
        }
    }
}