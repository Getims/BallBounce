using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.Base.UIAnimator.Animations
{
    [Serializable]
    public class Fade
    {
        [SerializeField]
        private FadeObject _fadeObject;

        [SerializeField]
        private float _targetValue;

        [SerializeField]
        [ShowIf(nameof(_fadeObject), FadeObject.CanvasGroup)]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        [ShowIf(nameof(_fadeObject), FadeObject.SpriteRenderer)]
        private SpriteRenderer _renderer;

        [SerializeField]
        [ShowIf(nameof(_fadeObject), FadeObject.UiImage)]
        private Image _image;

        public string GetObjectName()
        {
            GameObject gameObject = GetObject();
            if (gameObject != null)
                return gameObject.name;
            else
                return string.Empty;
        }

        public Tween GetTween(float animationTime, in bool instant)
        {
            switch (_fadeObject)
            {
                case FadeObject.CanvasGroup:
                    return GetForCanvasGroup(instant ? 0 : animationTime);
                case FadeObject.SpriteRenderer:
                    return GetForSpriteRenderer(instant ? 0 : animationTime);
                case FadeObject.UiImage:
                    return GetForUiImage(instant ? 0 : animationTime);
            }

            return null;
        }

        private Tween GetForCanvasGroup(float time)
        {
            return _canvasGroup.DOFade(_targetValue, time);
        }

        private Tween GetForSpriteRenderer(float time)
        {
            return _renderer.DOFade(_targetValue, time);
        }

        private Tween GetForUiImage(float time)
        {
            return _image.DOFade(_targetValue, time);
        }

        private GameObject GetObject()
        {
            switch (_fadeObject)
            {
                case FadeObject.CanvasGroup:
                    return _canvasGroup != null ? _canvasGroup.gameObject : null;
                case FadeObject.SpriteRenderer:
                    return _renderer != null ? _renderer.gameObject : null;
                case FadeObject.UiImage:
                    return _image != null ? _image.gameObject : null;
            }

            return null;
        }

        private enum FadeObject
        {
            CanvasGroup,
            SpriteRenderer,
            UiImage
        }
    }
}