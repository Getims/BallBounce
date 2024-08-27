using System.Collections;
using Main.Scripts.GameLogic.Sound;
using Main.Scripts.UI.Base;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Main.Scripts.UI.Common.FlyIcons
{
    public class FlyIconsSpawner : UIPanel
    {
        [Title("References")]
        [SerializeField, Required]
        private Sprite _iconSprite;

        [SerializeField, Required]
        private IconsPool _iconsPool;

        [Title("Settings")]
        [SerializeField, MinMaxSlider(minValue: 0, maxValue: 50, showFields: true)]
        private Vector2Int _iconsCount;

        [SerializeField, MinMaxSlider(minValue: 0, maxValue: 1, showFields: true)]
        private Vector2 _iconsSpawnDelay;

        [SerializeField, Required]
        private FlySettings _flySettings;

        [SerializeField]
        private float _startPointRandomOffset;

        private ISoundService _soundService;
        private Coroutine _animationCO;

        [Inject]
        public void Construct(ISoundService soundService)
        {
            _soundService = soundService;
        }

        public float GetAnimationTime()
        {
            return _flySettings.GetAnimationTime();
        }

        [Button]
        public void StartAnimation(Vector3 startPosition, Vector3 targetPosition)
        {
            if (_animationCO != null)
                StopCoroutine(_animationCO);

            _animationCO = StartCoroutine(SpawnIcons(startPosition, targetPosition));
        }

        private void Start()
        {
            _iconsPool.Initialize();
        }

        protected override void OnDestroy()
        {
            if (_animationCO != null)
                StopCoroutine(_animationCO);
        }

        private IEnumerator SpawnIcons(Vector3 startPosition, Vector3 targetPosition)
        {
            int iconsCount = Random.Range(_iconsCount.x, _iconsCount.y + 1);
            Vector2 startPositionRandomOffset = Vector2.zero;

            OnAnimationStart();
            
            for (int i = 0; i < iconsCount; i++)
            {
                if (i % 2 == 0)
                {
                    startPositionRandomOffset = Random.insideUnitCircle;
                    startPositionRandomOffset *= _startPointRandomOffset;
                }
                else
                {
                    startPositionRandomOffset = -startPositionRandomOffset;
                }

                startPosition += (Vector3) startPositionRandomOffset;

                SpawnIcon(startPosition, targetPosition);

                float delay = Random.Range(_iconsSpawnDelay.x, _iconsSpawnDelay.y);
                yield return new WaitForSeconds(delay);
            }
        }

        private void SpawnIcon(Vector3 startPosition, Vector3 targetPosition)
        {
            FlyIcon flyIcon = _iconsPool.Spawn();
            flyIcon.Setup(_iconSprite, _flySettings, OnIconMoved, () => OnFlyComplete(flyIcon));
            flyIcon.PlayAnimation(startPosition, targetPosition);
        }

        private void OnAnimationStart()
        {
            //_soundService.PlayRewardSound(); 
        }

        private void OnIconMoved()
        {
            _soundService.PlayAddCoinSound();
        }

        private void OnFlyComplete(FlyIcon flyIcon)
        {
            _iconsPool.Despawn(flyIcon);
        }
    }
}