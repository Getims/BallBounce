using System;
using Main.Scripts.Configs.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Sounds
{
    [Serializable]
    public class AudioClipsListConfig : ScriptableConfig
    {
        [Title("Background")]
        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_gameBackgroundMusic) + ")")]
        private AudioClipConfig _gameBackgroundMusic;

        [Title("Gameplay")]
        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_ballJump) + ")")]
        private AudioClipConfig _ballJump;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_ballPreMerge) + ")")]
        private AudioClipConfig _ballPreMerge;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_ballMerge) + ")")]
        private AudioClipConfig _ballMerge;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_prizeHit) + ")")]
        private AudioClipConfig _prizeHit;

        [Title("UI")]
        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_addCoin) + ")")]
        private AudioClipConfig _addCoin;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_reward) + ")")]
        private AudioClipConfig _reward;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_buttonClick) + ")")]
        private AudioClipConfig _buttonClick;

        [SerializeField, SuffixLabel("@GetLabel(" + nameof(_upgradeButtonClick) + ")")]
        private AudioClipConfig _upgradeButtonClick;

        public AudioClipConfig GameBackgroundMusic => _gameBackgroundMusic;
        public AudioClipConfig ButtonClick => _buttonClick;
        public AudioClipConfig BallJump => _ballJump;
        public AudioClipConfig BallPreMerge => _ballPreMerge;
        public AudioClipConfig BallMerge => _ballMerge;
        public AudioClipConfig PrizeHit => _prizeHit;
        public AudioClipConfig AddCoin => _addCoin;
        public AudioClipConfig UpgradeButtonClick => _upgradeButtonClick;
        public AudioClipConfig Reward => _reward;

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.SOUND_CATEGORY;

        private string GetLabel(AudioClipConfig clipConfig)
        {
            if (clipConfig.IsDisabled)
                return "- Disabled";
            if (clipConfig.AudioClip == null)
                return "- No Audio";
            return clipConfig.AudioClip.name;
        }
#endif
    }
}