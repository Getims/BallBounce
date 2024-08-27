using System;
using DG.Tweening;
using Main.Scripts.Core.Enums;
using Main.Scripts.UI.Base;
using Main.Scripts.UI.Common;
using TMPro;
using UnityEngine;

namespace Main.Scripts.UI.GameMenu.Timers
{
    public class Timer : UIPanel
    {
        [SerializeField]
        private BoosterType _boosterType;

        [SerializeField]
        private SunRays _sunRays;

        [SerializeField]
        private TextMeshProUGUI _valueTMP;

        private int _workTime;
        private float _currentTime = 0;
        private Tweener _timerTW;
        private Action _onTimeEnd;

        public BoosterType BoosterType => _boosterType;

        public void Initialize(int workTime, Action onTimeEnd)
        {
            _onTimeEnd = onTimeEnd;
            _workTime = workTime;
            _currentTime = 0;
            StopTimer();
        }

        public override void Show()
        {
            gameObject.SetActive(true);
            base.Show();
            _sunRays.PlayAnimation();
            StartTimer();
        }

        public override void Hide()
        {
            base.Hide(true);
            gameObject.SetActive(false);
            _sunRays.StopAnimation();
            StopTimer();
        }

        protected override void OnDestroy() => StopTimer();

        private void FixedUpdate() => UpdateTime();

        private void StartTimer()
        {
            if (_workTime <= 0)
                return;

            _timerTW?.Kill();

            _timerTW = DOTween.To(() => _currentTime, x => _currentTime = x, 1, _workTime)
                .SetEase(Ease.Linear)
                .OnComplete(OnTimeEnd);
        }

        private void StopTimer() =>
            _timerTW?.Kill();

        private void UpdateTime()
        {
            int currentTime = Mathf.RoundToInt(_workTime * (1 - _currentTime));
            _valueTMP.text = $"{currentTime}S";
        }

        private void OnTimeEnd()
        {
            _onTimeEnd?.Invoke();
            Hide();
        }
    }
}