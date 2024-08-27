using System;
using DG.Tweening;
using UnityEngine;

namespace Main.Scripts.UI.Base.UIAnimator.Animations
{
    [Serializable]
    public class Move
    {
        [SerializeField]
        private Transform _transform;

        [SerializeField]
        private bool _useLocalPosition = false;

        [SerializeField]
        private Vector3 _movePosition;

        [SerializeField]
        private MoveType _moveType;

        private RectTransform _rectTransform;

        public string GetObjectName()
        {
            if (_transform != null)
                return _transform.gameObject.name;
            else
                return string.Empty;
        }

        public Tween GetTween(float animationTime, bool instant)
        {
            _rectTransform = _transform.GetComponent<RectTransform>();

            if (_rectTransform == null)
                return GetForTransform(instant ? 0 : animationTime);
            else
                return GetForRectTransform(instant ? 0 : animationTime);
        }

        private Tween GetForRectTransform(float time)
        {
            Vector2 newPosition = _movePosition;
            if (_moveType == MoveType.AddPosition)
                newPosition = _rectTransform.anchoredPosition + (Vector2) _movePosition;

            return _rectTransform.DOAnchorPos(newPosition, time);
        }

        private Tween GetForTransform(float time)
        {
            Vector3 newPosition = _movePosition;
            Vector3 objectPosition = _useLocalPosition ? _transform.localPosition : _transform.position;

            if (_moveType == MoveType.AddPosition)
                newPosition = objectPosition + _movePosition;

            if (_useLocalPosition)
                return _transform.DOLocalMove(newPosition, time);
            else
                return _transform.DOMove(newPosition, time);
        }

        private enum MoveType
        {
            AddPosition,
            SetPosition
        }
    }
}