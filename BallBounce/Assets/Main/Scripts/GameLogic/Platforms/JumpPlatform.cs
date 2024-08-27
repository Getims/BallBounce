using DG.Tweening;
using UnityEngine;

namespace Main.Scripts.GameLogic.Platforms
{
    public class JumpPlatform : Platform
    {
        [SerializeField]
        private ShapeAnimator _animator;

        [SerializeField, Min(0.1f)]
        private float _showTime = 0.25f;

        private bool _isInitialized = false;
        private Vector3 _startScale;
        private Tweener _showTW;

        public void Show()
        {
            _showTW?.Kill();
            _showTW = transform.DOScale(_startScale, _showTime)
                .SetEase(Ease.OutSine);
        }

        public void Hide()
        {
            Initialize();
            _showTW?.Kill();
            transform.localScale = Vector3.zero;
        }

        public override void BallHit() =>
            _animator.Move();

        public override bool CanGiveMoney() => true;

        private void Start()
        {
            _animator.Initialize();
            Initialize();
        }

        private void OnDestroy()
        {
            _showTW?.Kill();
        }

        private void Initialize()
        {
            if (_isInitialized)
                return;
            _startScale = transform.localScale;
            _isInitialized = true;
        }
    }
}