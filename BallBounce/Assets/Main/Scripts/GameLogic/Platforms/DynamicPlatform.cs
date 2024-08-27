using System;
using DG.Tweening;
using Main.Scripts.GameLogic.GameFlow;
using UnityEngine;

namespace Main.Scripts.GameLogic.Platforms
{
    public class DynamicPlatform : MonoBehaviour
    {
        [SerializeField, Min(0.1f)]
        private float _showTime = 0.25f;

        [SerializeField]
        private PlatformTutorialHand _tutorialHand;

        private int _platformId = 0;
        private Action<int, Vector3> _onChangePlatformPosition;
        private bool _isInitialized = false;
        private Vector3 _startScale;
        private Tweener _showTW;
        private Vector3 _lastPosition;
        private GameZone _gameZone;
        private bool _tutorialShown = false;

        public void Initialize(GameZone gameZone, int platformId, Action<int, Vector3> onChangePlatformPosition,
            bool needTutorial)
        {
            _gameZone = gameZone;
            _platformId = platformId;
            _startScale = transform.localScale;
            _lastPosition = transform.position;

            _onChangePlatformPosition = onChangePlatformPosition;
            _onChangePlatformPosition?.Invoke(_platformId, _lastPosition);

            _tutorialShown = !needTutorial;
            _isInitialized = true;
        }

        public void Show()
        {
            if (!_isInitialized)
                return;

            _showTW?.Kill();
            _showTW = transform.DOScale(_startScale, _showTime)
                .SetEase(Ease.OutSine)
                .OnComplete(ShowHand);
        }

        public void Hide()
        {
            if (!_isInitialized)
                return;

            _showTW?.Kill();
            transform.localScale = Vector3.zero;
            _tutorialHand.Hide();
        }

        private void OnDestroy() =>
            _showTW?.Kill();

        private void FixedUpdate()
        {
            FixPosition();
            TryToSavePosition();
        }

        private void ShowHand()
        {
            if (_tutorialShown)
                return;

            _tutorialShown = true;
            _tutorialHand.ShowHand();
        }

        private void FixPosition()
        {
            Vector3 currentPosition = transform.position;
            if (currentPosition.x < _gameZone.MinX)
                currentPosition.x = _gameZone.MinX;
            if (currentPosition.x > _gameZone.MaxX)
                currentPosition.x = _gameZone.MaxX;
            if (currentPosition.y < _gameZone.MinY)
                currentPosition.y = _gameZone.MinY;
            if (currentPosition.y > _gameZone.MaxY)
                currentPosition.y = _gameZone.MaxY;

            transform.position = currentPosition;
        }

        private void TryToSavePosition()
        {
            Vector3 currentPosition = transform.position;
            if (_lastPosition == currentPosition)
                return;

            _tutorialHand.Hide();
            _lastPosition = currentPosition;
            _onChangePlatformPosition?.Invoke(_platformId, _lastPosition);
        }
    }
}