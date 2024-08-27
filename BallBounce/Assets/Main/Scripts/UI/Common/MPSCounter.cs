using Main.Scripts.Data.Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace Main.Scripts.UI.Common
{
    public class MPSCounter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _counter;

        [SerializeField, Min(0)]
        private float _checkTime = 3;

        private IProgressDataService _progressDataService;
        private float _lastMoney;
        private float _lastCheck;
        private float _lastMPS;

        [Inject]
        public void Construct(IProgressDataService progressDataService)
        {
            _progressDataService = progressDataService;
            Initialize();
        }

        private void Initialize()
        {
            _lastMoney = _progressDataService.LevelProgress;
            _lastCheck = Time.time;
            _lastMPS = 1;
            SetMPS(1);
        }

        private void FixedUpdate()
        {
            if (_lastCheck + _checkTime >= Time.time)
                return;

            CalculateMPS();
            SetMPS(_lastMPS);

            _lastCheck = Time.time;
        }

        private void CalculateMPS()
        {
            float currentMoney = _progressDataService.LevelProgress;
            float moneyDelta = currentMoney - _lastMoney;
            float deltaTime = Time.time - _lastCheck;
            float mps = moneyDelta / deltaTime;
            mps = (mps + _lastMPS) * 0.5f;

            _lastMPS = mps;
            _lastMoney = currentMoney;
        }

        private void SetMPS(float mps)
        {
            mps = Mathf.Round(mps);
            if (mps < 1)
                mps = 1;

            _counter.text = ((int) mps).ToString();
        }
    }
}