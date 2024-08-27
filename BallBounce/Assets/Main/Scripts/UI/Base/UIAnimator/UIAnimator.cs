using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Animation = Main.Scripts.UI.Base.UIAnimator.Animations.Animation;

namespace Main.Scripts.UI.Base.UIAnimator
{
    public class UIAnimator : MonoBehaviour
    {
        [SerializeField]
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "@ElementName", Expanded = true)]
        private List<Animation> _animations = new List<Animation>();

        [SerializeField]
        private bool _isLooped = false;

        private Sequence _sequence;
        private Coroutine _playCO;

        public event Action OnComplete;

        [Button]
        public void Play()
        {
            _sequence.Kill();
            _sequence = BuildSequence();
            
            _sequence.SetLoops(_isLooped ? -1 : 1)
                .Play()
                .OnComplete(OnAnimationComplete);
        }

        public void Stop(bool complete = false, bool completeWithCallbacks = false)
        {
            if (complete)
                _sequence.Complete(completeWithCallbacks);
            else
                _sequence.Kill();
        }

        private void OnDestroy() =>
            Stop();

        private Sequence BuildSequence()
        {
            Sequence sequence = DOTween.Sequence();
            foreach (Animations.Animation animation in _animations)
                animation.GetAction(ref sequence);
            return sequence;
        }

        private void OnAnimationComplete() =>
            OnComplete?.Invoke();

        [Button("Stop")]
        private void EditorStop() => Stop();

        [Button("Complete")]
        private void EditorComplete() => Stop();
    }


    internal enum AnimationType
    {
        Move,
        Rotate,
        Scale,
        Fade
    }

    internal enum PlayType
    {
        Append,
        Join
    }
}