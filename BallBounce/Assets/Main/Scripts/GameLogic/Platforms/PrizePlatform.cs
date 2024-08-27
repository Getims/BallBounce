using System;
using DG.Tweening;
using Main.Scripts.GameLogic.Sound;
using TMPro;
using UnityEngine;
using Zenject;

namespace Main.Scripts.GameLogic.Platforms
{
    public class PrizePlatform : Platform
    {
        [SerializeField]
        private Transform _iconContainer;

        [SerializeField]
        private TextMeshPro _hitsCounter;

        [SerializeField, Min(0.1f)]
        private float _showTime = 0.25f;

        [SerializeField]
        private bool _giveMoneyOnHit = false;

        [SerializeField, Min(0)]
        private float _hitTime;

        [SerializeField]
        private float _hitScale = 0.1f;

        private int _hitsCount = 0;
        private Vector3 _startScale = Vector3.one;
        private Tweener _showTW;
        private Tweener _scaleTW;
        private Action _onHitsOver;
        private ISoundService _soundService;

        [Inject]
        public void Construct(ISoundService soundService)
        {
            _soundService = soundService;
        }

        public void Initialize(int hitsCount, Action onHitsOver)
        {
            _onHitsOver = onHitsOver;
            _hitsCount = hitsCount;
            UpdateCounter();
        }

        public void Show()
        {
            _iconContainer.localScale = Vector3.one;
            _showTW?.Kill();
            _showTW = transform.DOScale(_startScale, _showTime)
                .SetEase(Ease.OutSine);
        }

        public void Hide(bool instant = false)
        {
            _showTW?.Kill();
            if (instant)
                transform.localScale = Vector3.zero;
            else
                _showTW = transform.DOScale(Vector3.zero, _showTime).SetEase(Ease.OutSine);
        }

        public override void BallHit()
        {
            _hitsCount--;
            UpdateCounter();
            PlayHitAnimation();
            _soundService.PlayPrizeHitSound();

            if (_hitsCount == 0)
                _onHitsOver?.Invoke();
        }

        public override bool CanGiveMoney() =>
            _giveMoneyOnHit;

        private void OnDestroy()
        {
            _showTW?.Kill();
            _scaleTW?.Kill();
        }

        private void UpdateCounter() =>
            _hitsCounter.text = _hitsCount.ToString();

        private void PlayHitAnimation()
        {
            _iconContainer.localScale = Vector3.one;
            _scaleTW?.Kill();
            _scaleTW = _iconContainer.DOPunchScale(Vector3.one * _hitScale, _hitTime, 1, 1);
        }
    }
}