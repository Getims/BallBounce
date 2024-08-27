using Main.Scripts.Configs.Sounds;

namespace Main.Scripts.GameLogic.Sound
{
    public interface ISoundService
    {
        bool IsSoundOn { get; }
        bool IsMusicOn { get; }
        
        void SwitchSoundState(bool isOn);
        void SwitchMusicState(bool isOn);
        void PlayGameBackgroundMusic();
        void PlayButtonClickSound();
        void PlayOneShot(AudioClipConfig clipConfig);
        void PlayWinSound();
        void PlayBallJumpSound();
        void PlayBallPreMergeSound();
        void PlayBallMergeSound();
        void PlayPrizeHitSound();
        void PlayAddCoinSound();
        void PlayUpgradeButtonClickSound();
        void PlayRewardSound();
    }
}