using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Main.Scripts.GameLogic.Money
{
    public class MoneyPlate : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro _value;

        [SerializeField, Min(0)]
        private float _moveTime;

        [SerializeField]
        private float _moveDistance;

        [SerializeField, Min(0)]
        private float _showTime;

        [SerializeField, Min(0)]
        private float _hideDelay;

        [SerializeField, Min(0)]
        private float _hideTime;

        private Ease _moveEase = Ease.InOutSine;
        private Sequence _sequence;

        public void Initialize(float value)
        {
            _value.text = $"+${value}";
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            float targetY = transform.localPosition.y + _moveDistance;

            _sequence.Append(_value.DOFade(0, 0)
                .SetEase(_moveEase));
            _sequence.Append(transform.DOLocalMoveY(targetY, _moveTime)
                .SetEase(_moveEase));
            _sequence.Join(_value.DOFade(1, _showTime)
                .SetEase(_moveEase));
            _sequence.Join(_value.DOFade(0, _hideTime)
                .SetDelay(_hideDelay + _showTime)
                .SetEase(_moveEase));

            _sequence.Play();
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}