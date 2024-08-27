using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.U2D;

namespace Main.Scripts.GameLogic.Platforms
{
    public class ShapeAnimator : MonoBehaviour
    {
        [SerializeField]
        private SpriteShapeController _spriteShape;

        [SerializeField]
        private Vector3 _bouncePosition;

        [SerializeField]
        private float _bounceHalfTime;

        [SerializeField, Min(0)]
        private float _bounceHeight;

        [SerializeField]
        private Ease _moveDownEase = Ease.InSine;

        [SerializeField]
        private Ease _moveUpEase = Ease.OutSine;

        private Sequence _sequence;
        private Vector3 _startPosition;
        private float _startHeight;
        private int _center;
        private bool _isInitialized;
        private bool _isMoving;

        public void Initialize()
        {
            _sequence = DOTween.Sequence();

            Spline spline = _spriteShape.spline;

            int pointsCount = spline.GetPointCount();
            _center = (int) (pointsCount * .5f);
            _startPosition = spline.GetPosition(_center);
            _startHeight = spline.GetHeight(_center);
            _isInitialized = true;
            _isMoving = false;
        }

        [Button]
        public void Move()
        {
            if (!_isInitialized)
                Initialize();

            Spline spline = _spriteShape.spline;

            _sequence.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Append(DOTween.To(() => _startPosition,
                    x => spline.SetPosition(_center, x), _bouncePosition, _bounceHalfTime)
                .SetEase(_moveDownEase));
            _sequence.Join(DOTween.To(() => _startHeight,
                    x => spline.SetHeight(_center, x), _bounceHeight, _bounceHalfTime)
                .SetEase(_moveDownEase));

            _sequence.Append(DOTween.To(() => _bouncePosition,
                    x => spline.SetPosition(_center, x), _startPosition, _bounceHalfTime)
                .SetEase(_moveUpEase));
            _sequence.Join(DOTween.To(() => _bounceHeight,
                    x => spline.SetHeight(_center, x), _startHeight, _bounceHalfTime)
                .SetEase(_moveUpEase));

            _isMoving = true;
            _sequence.Play()
                .OnComplete(() => _isMoving = false);
        }

        void FixedUpdate()
        {
            if (_isInitialized && _isMoving)
                _spriteShape.RefreshSpriteShape();
        }
    }
}