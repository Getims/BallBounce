using System;
using DG.Tweening;
using Main.Scripts.Configs.Levels;
using Main.Scripts.Core.Enums;
using Main.Scripts.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.GameMenu.Boosters
{
    public class BoostersButton : MonoBehaviour
    {
        [SerializeField]
        private BoosterType _boosterType;

        [SerializeField]
        private Button _button;

        [SerializeField]
        private Image _multiplierContainer;

        [SerializeField]
        private TextMeshProUGUI _multiplierValue;

        [SerializeField]
        private BoosterTimer _boosterTimer;

        [SerializeField]
        private SunRays _sunRays;

        [SerializeField, Min(0.1f)]
        private float _showTime = 0.25f;

        private BoosterConfig _boosterConfig;
        private Action<BoostersButton> _onBoosterButtonClick;
        private bool _isInitialized = false;
        private bool _isShowed = false;
        private Tweener _showTW;
        private Vector3 _startScale;

        public bool IsInitialized => _isInitialized;
        public BoosterType BoosterType => _boosterType;
        public bool IsShowed => _isShowed;
        public BoosterConfig BoosterConfig => _boosterConfig;

        public void Initialize(BoosterConfig boosterConfig, int onScreenTime,
            Action<BoostersButton> onBoosterButtonClick)
        {
            if (_boosterType == BoosterType.Null || boosterConfig == null)
            {
                _isInitialized = false;
                return;
            }

            _onBoosterButtonClick = onBoosterButtonClick;
            _boosterConfig = boosterConfig;
            _boosterTimer.Initialize(onScreenTime + 1);
            SetupMultiplier(boosterConfig);
            _startScale = transform.localScale;
            _isInitialized = true;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _isShowed = true;

            _showTW?.Kill();
            _showTW = transform.DOScale(_startScale, _showTime)
                .SetEase(Ease.OutSine);

            _boosterTimer.Reset();
            _boosterTimer.StartTimer();

            _sunRays.PlayAnimation();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _isShowed = false;

            _showTW?.Kill();
            transform.localScale = Vector3.zero;

            _boosterTimer.StopTimer();
            _sunRays.StopAnimation();
        }

        private void Start()
        {
            _button.onClick.AddListener(OnClick);
            _boosterTimer.OnTimeEndEvent += OnTimeEnd;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
            _boosterTimer.OnTimeEndEvent -= OnTimeEnd;
        }

        private void SetupMultiplier(BoosterConfig boosterConfig)
        {
            _multiplierValue.text = $"x{boosterConfig.Multiplier}";
            _multiplierValue.enabled = boosterConfig.Multiplier > 1;

            if (_multiplierContainer != null)
                _multiplierContainer.enabled = boosterConfig.Multiplier > 1;
        }

        private void OnClick()
        {
            _onBoosterButtonClick?.Invoke(this);
            Hide();
        }

        private void OnTimeEnd() => Hide();
    }
}