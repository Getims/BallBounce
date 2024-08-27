using System;
using DG.Tweening;
using UnityEngine;

namespace Main.Scripts.UI.Base.UIAnimator.Animations
{
    [Serializable]
    public class Scale
    {
        [SerializeField]
        private Transform _transform;

        [SerializeField]
        private Vector3 _targetScale;

        public Tween GetTween(float animationTime, bool instant)
        {
            return _transform.DOScale(_targetScale, instant ? 0 : animationTime);
        }

        public string GetObjectName()
        {
            if (_transform != null)
                return _transform.gameObject.name;
            else
                return string.Empty;
        }
    }
}