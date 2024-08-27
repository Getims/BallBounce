using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using Main.Scripts.Configs;
using Main.Scripts.Configs.Balls;
using Main.Scripts.Data;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic.GameFlow;
using Main.Scripts.GameLogic.Money;
using Main.Scripts.GameLogic.Sound;
using Main.Scripts.Infrastructure.Providers.Configs;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Main.Scripts.GameLogic.Balls
{
    public class BallsController : MonoBehaviour
    {
        [SerializeField]
        private LeanGameObjectPool _ballsPool;

        [SerializeField]
        private Transform _ballsContainer;

        [SerializeField]
        private Transform _ballsSpawnPoint;

        [SerializeField]
        private BallMerger _ballMerger;

        private BallProgressionConfig _ballProgressionConfig;
        private IProgressDataService _progressDataService;
        private GameZone _gameZone;
        private MoneyController _moneyController;

        private List<Ball> _balls = new List<Ball>();
        private bool _isInitialized = true;
        private int _spawnBallLevel = 0;
        private Coroutine _spawnDataBallsCO;
        private Coroutine _spawnBallsCO;
        private bool _canMergeBalls = false;

        public bool CanMergeBalls => _canMergeBalls;

        [Inject]
        public void Construct(ISoundService soundService, IBallProgressionConfigProvider ballProgressionConfigProvider,
            IProgressDataService progressDataService)
        {
            _progressDataService = progressDataService;
            _ballMerger.Initialize(soundService);
            _ballProgressionConfig = ballProgressionConfigProvider.Config;
        }

        public void Initialize(GameZone gameZone, MoneyController moneyController)
        {
            _moneyController = moneyController;
            _gameZone = gameZone;
            DisableAllBalls();
            _ballMerger.Initialize(_ballsSpawnPoint);
            _spawnBallLevel = 0;
            _isInitialized = true;
        }

        public void FirstBallSpawn()
        {
            if (!_isInitialized)
                return;

            if (!_progressDataService.BallsData.Any())
                SpawnBall();
            else
            {
                if (_spawnDataBallsCO != null)
                    StopCoroutine(_spawnDataBallsCO);
                _spawnDataBallsCO = StartCoroutine(SpawnBallsFromData());
            }
        }

        [Button]
        public void SpawnBall()
        {
            if (!_isInitialized)
                return;

            Ball ball = GetBallFromPool();
            ball.Initialize(_spawnBallLevel, GetBallConfig(_spawnBallLevel), _gameZone, OnBallMoveOutOfGameZone,
                OnBallHit);
            SaveBallToList(ball);
            UpdateBallsData();
            RecalculateMergeState();
        }

        public void SpawnBalls(int count)
        {
            if (_spawnBallsCO != null)
                StopCoroutine(_spawnBallsCO);
            _spawnBallsCO = StartCoroutine(SpawnBallsCoroutine(count));
        }

        [Button]
        public void MergeBalls()
        {
            Ball ballToUpgrade = null;
            Ball ballToRemove = null;
            int i = 0;

            for (; i < _balls.Count - 1; i++)
            {
                ballToUpgrade = _balls[i];
                ballToRemove = _balls[i + 1];

                if (ballToUpgrade.BallLevel == ballToRemove.BallLevel)
                    break;

                ballToUpgrade = null;
            }

            if (ballToUpgrade == null)
                return;

            int newLevel = ballToUpgrade.BallLevel + 1;

            ballToUpgrade.SetConfig(newLevel, ballToUpgrade.BallConfig);
            _balls.RemoveAt(i + 1);
            RecalculateSpawnLevel();
            UpdateBallsData();
            RecalculateMergeState();

            _ballMerger.Merge(ballToUpgrade, ballToRemove, newLevel, GetBallConfig(newLevel), () =>
            {
                OnBallMoveOutOfGameZone(ballToUpgrade);
                _ballsPool.Despawn(ballToRemove.gameObject);
                ballToRemove.Show();
            });
        }

        public void RespawnBalls()
        {
            DisableAllBalls();
            if (_spawnDataBallsCO != null)
                StopCoroutine(_spawnDataBallsCO);
            _spawnDataBallsCO = StartCoroutine(SpawnBallsFromData());
        }

        private void OnDestroy()
        {
            if (_spawnDataBallsCO != null)
                StopCoroutine(_spawnDataBallsCO);
            if (_spawnBallsCO != null)
                StopCoroutine(_spawnBallsCO);
            _isInitialized = false;
        }

        private void OnDrawGizmos() =>
            _ballMerger.DrawGizmos();

        private IEnumerator SpawnBallsFromData()
        {
            WaitForSeconds spawnPause = new WaitForSeconds(0.075f);
            IReadOnlyCollection<BallData> ballsData = _progressDataService.BallsData;
            foreach (BallData ballData in ballsData)
            {
                Ball ball = GetBallFromPool();
                ball.Initialize(ballData.BallLevel, GetBallConfig(ballData.BallLevel), _gameZone,
                    OnBallMoveOutOfGameZone,
                    OnBallHit);
                SaveBallToList(ball);
                yield return spawnPause;
            }

            RecalculateSpawnLevel();
            RecalculateMergeState();
        }

        private IEnumerator SpawnBallsCoroutine(int count)
        {
            WaitForSeconds spawnPause = new WaitForSeconds(0.075f);
            for (int i = 0; i < count; i++)
            {
                SpawnBall();
                yield return spawnPause;
            }
        }

        private void RecalculateSpawnLevel()
        {
            int maxLevel = _balls[0].BallLevel;
            int newMinLevel = maxLevel - _ballProgressionConfig.CreationMinLevelDelta;

            if (newMinLevel > _spawnBallLevel)
            {
                _spawnBallLevel = newMinLevel;
                var minBallConfig = GetBallConfig(_spawnBallLevel);
                foreach (Ball ball in _balls)
                {
                    if (ball.BallLevel < _spawnBallLevel)
                        ball.SetConfig(_spawnBallLevel, minBallConfig, true);
                }
            }
        }

        private void RecalculateMergeState()
        {
            _canMergeBalls = false;
            for (int i = 0; i < _balls.Count - 1; i++)
            {
                if (_balls[i].BallLevel == _balls[i + 1].BallLevel)
                {
                    _canMergeBalls = true;
                    return;
                }
            }
        }

        private Ball GetBallFromPool()
        {
            GameObject ballInstance = _ballsPool.Spawn(GetSpawnPosition(), Quaternion.identity, _ballsContainer);
            return ballInstance.GetComponent<Ball>();
        }

        private Vector3 GetSpawnPosition()
        {
            Vector3 position = _ballsSpawnPoint.position;
            position.z = _ballsContainer.position.z;
            return position;
        }

        private BallConfig GetBallConfig(int level) =>
            _ballProgressionConfig.GetBallsConfig(level);

        private void SaveBallToList(Ball ball)
        {
            int index = _balls.FindIndex(b => b.BallLevel < ball.BallLevel);
            if (index == -1)
                _balls.Add(ball);
            else
                _balls.Insert(index, ball);
        }

        private void UpdateBallsData()
        {
            List<BallData> ballsData = new List<BallData>();
            foreach (Ball ball in _balls)
                ballsData.Add(new BallData(ball.BallLevel));

            _progressDataService.SetBallsData(ballsData);
        }

        private void DisableAllBalls()
        {
            _ballsPool.DespawnAll();
            foreach (Ball ball in _balls)
            {
                ball.Hide(false, true);
                ball.StopMovement();
            }

            _balls.Clear();
        }

        private void OnBallMoveOutOfGameZone(Ball ball)
        {
            ball.Hide(false);
            ball.transform.position = GetSpawnPosition();
            ball.InitializeMovement();
        }

        private void OnBallHit(Ball ball)
        {
            float money = _ballProgressionConfig.MoneyPerBallHitConfig.Get(ball.BallLevel);
            _moneyController.AddMoney(ball.transform.position, money);
        }
    }
}