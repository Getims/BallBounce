using System;
using DG.Tweening;
using UnityEngine;

namespace Main.Scripts.GameLogic.Balls
{
    [Serializable]
    internal class SpawnAnimator
    {
        [SerializeField, Min(0)]
        private float _time;

        [SerializeField, Min(0)]
        private float _delay;

        [SerializeField, Min(0)]
        private float _move;

        [SerializeField]
        private Ease _ease = Ease.InSine;

        private Tweener _animationTW;

        public void Play(Transform ball, Action OnComplete)
        {
            _animationTW?.Kill();

            float newPosition = ball.localPosition.y - _move;
            _animationTW = ball.DOLocalMoveY(newPosition, _time)
                .SetDelay(_delay)
                .SetEase(_ease)
                .OnComplete(() => OnComplete?.Invoke());
        }

        public void Stop()
        {
            _animationTW?.Kill();
        }
    }
}