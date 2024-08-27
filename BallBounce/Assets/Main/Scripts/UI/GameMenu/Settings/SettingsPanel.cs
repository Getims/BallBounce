using Main.Scripts.GameLogic.Sound;
using Main.Scripts.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.UI.GameMenu.Settings
{
    public class SettingsPanel : UIPanel
    {
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private SoundButton _soundButton;

        [SerializeField]
        private SoundButton _musicButton;

        private ISoundService _soundService;

        public override void Show()
        {
            base.Show();
            UpdateInfo();
        }

        protected virtual void Start() =>
            _closeButton.onClick.AddListener(OnCloseClick);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _closeButton.onClick.RemoveListener(OnCloseClick);
        }

        private void UpdateInfo()
        {
            _soundButton.UpdateInfo();
            _musicButton.UpdateInfo();
        }

        private void OnCloseClick()
        {
            Hide();
        }
    }
}