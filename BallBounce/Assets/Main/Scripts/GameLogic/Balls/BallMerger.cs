using System;
using DG.Tweening;
using Main.Scripts.Configs.Balls;
using Main.Scripts.GameLogic.Sound;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.GameLogic.Balls
{
    [Serializable]
    public class BallMerger
    {
        [SerializeField]
        private ParticleSystem _mergeParticles;

        [SerializeField, Min(0)]
        private float _scale = 3f;

        [Title("Border settings")]
        [SerializeField]
        private int _positionZ = -10;

        [SerializeField, Min(0)]
        private float _borderPosition = 3;

        [SerializeField, Min(0)]
        private float _moveToBordersTime = 0.15f;

        [Title("Center settings")]
        [SerializeField]
        private float _centerY = -10;

        [SerializeField, Min(0)]
        private float _moveToCenterDelay = 0.2f;

        [SerializeField, Min(0)]
        private float _moveToCenterTime = 0.45f;

        [SerializeField]
        private Ease _moveToCenterEase = Ease.OutSine;

        [Title("Up settings")]
        [SerializeField, Min(0)]
        private float _moveUpTime = 0.45f;

        [SerializeField, Min(0)]
        private float _moveUpDelay = 0.25f;

        [SerializeField]
        private Ease _moveUpEase = Ease.OutSine;

        private Sequence _sequence;
        private Transform _spawnPoint;
        private ISoundService _soundService;

        public void Initialize(Transform spawnPoint)
        {
            _spawnPoint = spawnPoint;
        }

        public void Initialize(ISoundService soundService)
        {
            _soundService = soundService;
        }

        public void Merge(Ball ballToUpgrade, Ball ballToRemove, int newLevel, BallConfig upgradeConfig,
            Action onMergeComplete)
        {
            Vector3 leftBallPosition = new Vector3(-_borderPosition, _centerY, _positionZ);
            Vector3 rightBallPosition = new Vector3(_borderPosition, _centerY, _positionZ);
            Vector3 centerPosition = new Vector3(0, _centerY, _positionZ);
            Vector3 upPosition = _spawnPoint.position;
            upPosition.z = _positionZ;

            Transform upgradeTransform = ballToUpgrade.transform;
            Transform removeTransform = ballToRemove.transform;

            Vector3 startScale = upgradeTransform.localScale;
            Vector3 targetScale = startScale * _scale;

            _sequence?.Complete();
            ballToUpgrade.StopMovement();
            ballToRemove.StopMovement();
            _sequence = DOTween.Sequence();

            _soundService.PlayBallPreMergeSound();
            _sequence.Append(upgradeTransform.DOMove(leftBallPosition, _moveToBordersTime));
            _sequence.Join(upgradeTransform.DOScale(targetScale, _moveToBordersTime));
            _sequence.Join(removeTransform.DOMove(rightBallPosition, _moveToBordersTime));
            _sequence.Join(removeTransform.DOScale(targetScale, _moveToBordersTime));

            _sequence.Append(upgradeTransform.DOMove(centerPosition, _moveToCenterTime)
                .SetEase(_moveToCenterEase)
                .SetDelay(_moveToCenterDelay)
                .OnComplete(() =>
                {
                    _mergeParticles.Play();
                    _soundService.PlayBallMergeSound();
                    ballToUpgrade.SetConfig(newLevel, upgradeConfig);
                    ballToRemove.Hide();
                }));

            _sequence.Join(removeTransform.DOMove(centerPosition, _moveToCenterTime)
                .SetEase(_moveToCenterEase));

            _sequence.Append(upgradeTransform.DOMove(upPosition, _moveUpTime)
                .SetDelay(_moveUpDelay)
                .SetEase(_moveUpEase)
                .OnComplete(() => onMergeComplete?.Invoke()));
            _sequence.Join(upgradeTransform.DOScale(startScale, _moveUpTime * .9f));
        }

        public void DrawGizmos()
        {
            Vector3 leftPosition = new Vector3(-_borderPosition, _centerY, _positionZ);
            Vector3 rightPosition = new Vector3(_borderPosition, _centerY, _positionZ);
            Vector3 centerPosition = new Vector3(0, _centerY, _positionZ);
            float radius = .5f;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(leftPosition, radius);
            Gizmos.DrawWireSphere(rightPosition, radius);
            Gizmos.DrawWireSphere(centerPosition, radius);
        }
    }
}