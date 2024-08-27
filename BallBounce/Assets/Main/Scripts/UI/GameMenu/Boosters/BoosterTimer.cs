using System;
using DG.Tweening;
using Main.Scripts.Core.Utilities;
using TMPro;
using UnityEngine;

namespace Main.Scripts.UI.GameMenu.Boosters
{
    internal class BoosterTimer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _valueTMP;

        private float _showTime = 0;
        private float _currentTime = 0;
        private Tweener _timerTW;

        public event Action OnTimeEndEvent;

        public void Initialize(float time)
        {
            _showTime = time;
            _currentTime = 0;
            _timerTW?.Kill();
        }

        public void StartTimer()
        {
            if (_showTime <= 0)
                return;

            _timerTW?.Kill();

            _timerTW = DOTween.To(() => _currentTime, x => _currentTime = x, 1, _showTime)
                .SetEase(Ease.Linear)
                .OnComplete(OnTimeEnd);
        }

        public void StopTimer() =>
            _timerTW?.Kill();

        public void Reset()
        {
            _currentTime = 0;
            _timerTW?.Kill();

            _timerTW = DOTween.To(() => _currentTime, x => _currentTime = x, 1, _showTime)
                .SetEase(Ease.Linear)
                .OnComplete(OnTimeEnd);
        }

        private void OnDestroy() => StopTimer();

        private void FixedUpdate() => UpdateTime();

        private void UpdateTime()
        {
            int currentTime = (int) (_showTime * (1 - _currentTime));
            NumbersExtensions.ConvertToMinutes(currentTime, out string value);
            _valueTMP.text = value;
        }

        private void OnTimeEnd() => OnTimeEndEvent?.Invoke();
    }
}