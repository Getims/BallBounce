using System;
using Main.Scripts.GameLogic.Sound;
using Main.Scripts.UI.Base;
using Main.Scripts.UI.Base.UIAnimator;
using Main.Scripts.UI.Common;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Main.Scripts.UI.GameMenu.Prize
{
    public class PrizePanel : UIPanel
    {
        [SerializeField]
        private Button _unlockButton;

        [SerializeField]
        private Button _collectButton;

        [SerializeField]
        private SunRays _sunRays;

        [SerializeField]
        private Transform _moneyIcon;

        [SerializeField]
        private TextMeshProUGUI _rewardTMP;

        [SerializeField]
        private UIAnimator _resetAnimator;

        [SerializeField]
        private UIAnimator _showAnimator;

        [SerializeField]
        private UIAnimator _openAnimator;

        private bool _rewardCollected = false;
        private int _reward = 0;
        private ISoundService _soundService;

        public event Action OnPrizeCollectClick;

        public Vector3 MoneyIconPosition => _moneyIcon.position;
        public bool RewardCollected => _rewardCollected;
        public int Reward => _reward;

        [Inject]
        public void Construct(ISoundService soundService)
        {
            _soundService = soundService;
        }

        public void Initialize(int reward)
        {
            _rewardCollected = false;
            _reward = reward;
            _rewardTMP.text = $"{reward}";
        }

        [Button]
        public override void Show()
        {
            base.Show();
            _resetAnimator.Play();
            _resetAnimator.OnComplete += OnReset;
        }

        [Button]
        public override void Hide()
        {
            base.Hide();
            _resetAnimator.OnComplete -= OnReset;
            _showAnimator.OnComplete -= OnUnlockButtonClick;
            _sunRays.StopAnimation();
            _reward = 0;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _unlockButton.onClick.RemoveListener(OnUnlockButtonClick);
            _collectButton.onClick.RemoveListener(OnCollectButtonClick);
            _showAnimator.OnComplete -= OnUnlockButtonClick;
        }

        private void Start()
        {
            _unlockButton.onClick.AddListener(OnUnlockButtonClick);
            _collectButton.onClick.AddListener(OnCollectButtonClick);
        }

        private void OnReset()
        {
            _resetAnimator.OnComplete -= OnReset;
            _showAnimator.Play();
            _showAnimator.OnComplete += OnUnlockButtonClick;
            _sunRays.PlayAnimation();
            _soundService.PlayRewardSound();
        }

        private void OnUnlockButtonClick()
        {
            _showAnimator.OnComplete -= OnUnlockButtonClick;
            _showAnimator.Stop();
            _openAnimator.Play();
        }

        private void OnCollectButtonClick()
        {
            OnPrizeCollectClick?.Invoke();
            Hide();
            _rewardCollected = true;
        }
    }
}