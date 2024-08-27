using System;
using Lean.Pool;
using Main.Scripts.Configs.Levels;
using Main.Scripts.Data.Services;
using UnityEngine;
using Zenject;

namespace Main.Scripts.GameLogic.Money
{
    public class MoneyController : MonoBehaviour
    {
        [SerializeField]
        private LeanGameObjectPool _moneyPool;

        [SerializeField]
        private Transform _moneyContainer;

        [SerializeField]
        private float _spawnOffsetY;

        [SerializeField, Min(0)]
        private float _despawnTime = 1f;

        private IProgressDataService _progressDataService;
        private IPlayerDataService _playerDataService;
        private float _targetMoney;
        private Action _onReachMoneyTarget;
        private float _multiplier = 1;

        [Inject]
        public void Construct(IProgressDataService progressDataService, IPlayerDataService playerDataService)
        {
            _playerDataService = playerDataService;
            _progressDataService = progressDataService;
        }

        public void Initialize(LevelConfig levelConfig, Action OnReachMoneyTarget)
        {
            _moneyPool.DespawnAll();

            _onReachMoneyTarget = OnReachMoneyTarget;
            _targetMoney = levelConfig.TargetMoney;
        }

        public void SetMultiplier(float multiplier)
        {
            if (multiplier < 1)
                multiplier = 1;
            _multiplier = multiplier;
        }

        public void AddMoney(Vector3 ballPosition, float value)
        {
            ShowMoneyPlate(ballPosition, value * _multiplier);
            UpdateProgress(value * _multiplier);
        }

        private void ShowMoneyPlate(Vector3 ballPosition, float value)
        {
            GameObject instance =
                _moneyPool.Spawn(GetSpawnPosition(ballPosition), Quaternion.identity, _moneyContainer);

            if (instance == null)
                return;

            MoneyPlate moneyPlate = instance.GetComponent<MoneyPlate>();

            moneyPlate.Initialize(value);
            _moneyPool.Despawn(instance, _despawnTime);
        }

        private void UpdateProgress(float value)
        {
            float currentProgress = _progressDataService.LevelProgress;
            currentProgress += value;

            _progressDataService.SetLevelProgress(currentProgress);
            _playerDataService.AddMoney((int) value);

            if (currentProgress >= _targetMoney)
                _onReachMoneyTarget?.Invoke();
        }

        private Vector3 GetSpawnPosition(Vector3 ballPosition)
        {
            ballPosition.z = _moneyContainer.position.z;
            ballPosition.y += _spawnOffsetY;
            return ballPosition;
        }
    }
}