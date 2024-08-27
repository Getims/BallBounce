using System;
using DG.Tweening;
using UnityEngine;

namespace Main.Scripts.UI.Base.UIAnimator.Animations
{
    [Serializable]
    public class Rotate
    {
        [SerializeField]
        private Transform _transform;

        [SerializeField]
        private Vector3 _rotation;

        [SerializeField]
        private RotateMode _rotateMode;

        public Tween GetTween(float animationTime, bool instant)
        {
            return _transform.DOLocalRotate(_rotation, instant ? 0 : animationTime, _rotateMode);
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