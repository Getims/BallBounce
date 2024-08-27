using System;
using System.Globalization;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.UI.Base.UIAnimator.Animations
{
    [Serializable]
    public class Animation
    {
        [SerializeField]
        private AnimationType _animationType;

        [SerializeField]
        private PlayType _playType;

        [SerializeField]
        private bool _instant = false;

        [SerializeField, Min(0), LabelWidth(125)]
        [HideIf(nameof(_instant))]
        private float _animationTime;

        [SerializeField, Min(0), LabelWidth(125)]
        [HideIf(nameof(_instant))]
        private float _startDelay;

        [SerializeField, LabelWidth(125)]
        [HideIf(nameof(_instant))]
        private Ease _animationEase;

        [SerializeField]
        [ShowIf(nameof(_animationType), AnimationType.Move)]
        private Move _move;

        [SerializeField]
        [ShowIf(nameof(_animationType), AnimationType.Rotate)]
        private Rotate _rotate;

        [SerializeField]
        [ShowIf(nameof(_animationType), AnimationType.Scale)]
        private Scale _scale;

        [SerializeField]
        [ShowIf(nameof(_animationType), AnimationType.Fade)]
        private Fade _fade;

        private string ElementName =>
            $"{_animationType} {GetObjectName()}, {_playType}," +
            $" playtime: {(_instant ? "instant" : _animationTime.ToString("G", new CultureInfo("en-US")))}" +
            $"{(_startDelay > 0 ? (", delay: " + _startDelay.ToString()) : "")}";

        public void GetAction(ref Sequence sequence)
        {
            if (_playType == PlayType.Append)
                sequence.Append(GetTween().SetDelay(_startDelay).SetEase(_animationEase));
            else
                sequence.Join(GetTween().SetDelay(_startDelay).SetEase(_animationEase));
        }

        private Tween GetTween()
        {
            switch (_animationType)
            {
                case AnimationType.Move:
                    return _move.GetTween(_animationTime, _instant);
                case AnimationType.Rotate:
                    return _rotate.GetTween(_animationTime, _instant);
                case AnimationType.Scale:
                    return _scale.GetTween(_animationTime, _instant);
                case AnimationType.Fade:
                    return _fade.GetTween(_animationTime, _instant);
            }

            return null;
        }

        private string GetObjectName()
        {
            switch (_animationType)
            {
                case AnimationType.Move:
                    return _move.GetObjectName();
                case AnimationType.Rotate:
                    return _rotate.GetObjectName();
                case AnimationType.Scale:
                    return _scale.GetObjectName();
                case AnimationType.Fade:
                    return _fade.GetObjectName();
            }

            return string.Empty;
        }
    }
}