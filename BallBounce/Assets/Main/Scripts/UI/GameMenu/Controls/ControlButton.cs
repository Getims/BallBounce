using System;
using Main.Scripts.GameLogic.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.GameMenu.Controls
{
    public class ControlButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private Button _adsButton;

        [SerializeField]
        private GameObject _lockedContainer;

        [SerializeField]
        private GameObject _unlockedContainer;

        [SerializeField]
        private GameObject _adsContainer;

        [SerializeField]
        private TextMeshProUGUI _pricePlate;

        private ISoundService _soundService;
        private ControlLogic _controlLogic;
        private Action _onClick;
        private Action _onAdsClick;
        private bool _isLocked;
        private int _cost;

        public int Cost => _cost;

        [Inject]
        public void Construct(ISoundService soundService) =>
            _soundService = soundService;

        public void Initialize(ControlLogic controlLogic, Action OnClick, Action OnAdsClick)
        {
            _onAdsClick = OnAdsClick;
            _onClick = OnClick;
            _controlLogic = controlLogic;
            _controlLogic.Initialize(SetState, SetAds, SetPrice);
        }

        public void UpdateState() =>
            _controlLogic.UpdateState();

        private void Start()
        {
            _button.onClick.AddListener(OnClick);
            _adsButton.onClick.AddListener(OnAdsClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
            _adsButton.onClick.RemoveListener(OnAdsClick);
        }

        private void SetPrice(int value)
        {
            _cost = value;
            _pricePlate.text = $"$ {value}";
        }

        private void SetState(bool isActive)
        {
            _lockedContainer.SetActive(!isActive);
            _unlockedContainer.SetActive(isActive);
            _isLocked = !isActive;
        }

        private void SetAds(bool isActive) =>
            _adsContainer.SetActive(isActive);

        private void OnClick()
        {
            if (_isLocked)
                return;

            _soundService?.PlayUpgradeButtonClickSound();
            _onClick?.Invoke();
            _controlLogic?.OnClick();
        }

        private void OnAdsClick()
        {
            _soundService?.PlayUpgradeButtonClickSound();
            _onAdsClick?.Invoke();
            _controlLogic?.OnAdsClick();
        }
    }
}