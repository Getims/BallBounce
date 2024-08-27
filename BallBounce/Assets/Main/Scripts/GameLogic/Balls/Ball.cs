using System;
using Main.Scripts.Configs.Balls;
using Main.Scripts.Core.Utilities;
using Main.Scripts.GameLogic.GameFlow;
using Main.Scripts.GameLogic.Sound;
using UnityEngine;
using Zenject;

namespace Main.Scripts.GameLogic.Balls
{
    public class Ball : MonoBehaviour
    {
        [SerializeField]
        private BallMovement _ballMovement;

        [SerializeField]
        private BallVisual _ballVisual;

        [SerializeField]
        private SpawnAnimator _spawnAnimator = new SpawnAnimator();

        private bool _isInitialized = false;
        private int _ballLevel = -1;
        private BallConfig _ballConfig;
        private GameZone _gameZone;
        private ISoundService _soundService;
        private Action<Ball> _onBallMoveOutOfGameZone;
        private Action<Ball> _onBallHit;

        public int BallLevel => _ballLevel;
        public BallConfig BallConfig => _ballConfig;

        [Inject]
        public void Construct(ISoundService soundService)
        {
            _soundService = soundService;
        }

        public void Initialize(int ballLevel, BallConfig ballConfig, GameZone gameZone,
            Action<Ball> onBallMoveOutOfGameZone, Action<Ball> onBallHit)
        {
            _onBallHit = onBallHit;
            _ballLevel = ballLevel;
            _ballConfig = ballConfig;
            _onBallMoveOutOfGameZone = onBallMoveOutOfGameZone;
            _gameZone = gameZone;

            _ballMovement.Initialize();
            _ballVisual.Initialize(_ballConfig);

            StopMovement();
            PlaySpawnAnimation();
        }

        public void InitializeMovement()
        {
            if (_ballLevel < 0)
                return;

            _ballMovement.Initialize();
            PlaySpawnAnimation();
        }

        public void StopMovement() => _isInitialized = false;

        public void SetConfig(int ballLevel, BallConfig ballConfig, bool showParticles = false)
        {
            if (_ballLevel < 0)
                return;

            _ballLevel = ballLevel;
            _ballConfig = ballConfig;
            _ballVisual.Initialize(_ballConfig);
            if (showParticles)
                _ballVisual.ShowUpgradeParticles();
        }

        public void Show(bool showBall = true, bool showTrail = true) =>
            _ballVisual.Show(showBall, showTrail);

        public void Hide(bool hideBall = true, bool hideTrail = true) =>
            _ballVisual.Hide(hideBall, hideTrail);

        private void Start()
        {
            _ballMovement.OnBallHit += OnBallHit;
            _ballMovement.OnStuck += OnBallStuck;
        }

        private void OnDestroy() =>
            _spawnAnimator.Stop();

        private void FixedUpdate()
        {
            if (!_isInitialized)
                return;

            if (_gameZone != null)
            {
                if (!Utils.IncludesInInterval(transform.position.x, new Vector2(_gameZone.MinX, _gameZone.MaxX))
                    || transform.position.y < _gameZone.MinY)
                {
                    OnMoveOutOfGameZone();
                    return;
                }
            }

            _ballMovement.Move();
        }

        private void PlaySpawnAnimation()
        {
            _ballVisual.SetRotationSpeed(new Vector2(Utils.RandomValue(20, -20), 50));
            _spawnAnimator.Play(transform, () =>
            {
                _isInitialized = true;
                _ballVisual.Show();
            });
        }

        private void OnBallHit(bool isJumpPlatform)
        {
            _ballVisual.SetRotationSpeed(_ballMovement.CurrentVelocity);
            if (isJumpPlatform)
            {
                _soundService.PlayBallJumpSound();
                _onBallHit?.Invoke(this);
            }
        }

        private void OnMoveOutOfGameZone()
        {
            _isInitialized = false;
            _onBallMoveOutOfGameZone.Invoke(this);
        }

        private void OnBallStuck() => OnMoveOutOfGameZone();
    }
}