using System;
using DG.Tweening;
using UnityEngine;

namespace Main.Scripts.UI.Common
{
    [Serializable]
    internal class SunRays : MonoBehaviour
    {
        [SerializeField]
        private Transform _raysIcon;

        [SerializeField, Min(0)]
        private float _duration = 2f;

        [SerializeField]
        private bool _playOnStart = true;

        private Ease _ease = Ease.Linear;
        private float _degrees = 360;
        private Tweener _rotateTW;

        public void PlayAnimation() => Rotate();

        public void StopAnimation() => _rotateTW.Kill();

        private void Start()
        {
            if (_playOnStart)
                PlayAnimation();
        }

        private void OnDestroy() => StopAnimation();

        private void Rotate()
        {
            _rotateTW.Kill();
            Vector3 rotation = Vector3.zero;
            rotation.z = _degrees;
            _rotateTW = _raysIcon.DOLocalRotate(rotation, _duration, RotateMode.LocalAxisAdd)
                .SetEase(_ease)
                .SetLoops(-1, LoopType.Incremental);
        }
    }
}