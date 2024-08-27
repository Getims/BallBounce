using Main.Scripts.UI.Base.UIAnimator;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.GameLogic.Platforms
{
    public class PlatformTutorialHand : MonoBehaviour
    {
        [SerializeField]
        private UIAnimator _animator;

        [SerializeField]
        private UIAnimator _hideAnimator;

        private bool _isShown = false;

        [Button]
        public void ShowHand()
        {
            if (_isShown)
                return;

            _animator.Play();
            _isShown = true;
        }

        public void Hide()
        {
            if (!_isShown)
                return;

            _isShown = false;
            _animator.Stop();
            _hideAnimator.Play();
        }
    }
}