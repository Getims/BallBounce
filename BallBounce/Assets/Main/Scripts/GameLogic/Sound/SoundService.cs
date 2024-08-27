using Main.Scripts.Configs.Sounds;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.Infrastructure.Providers.Events;

namespace Main.Scripts.GameLogic.Sound
{
    public class SoundService : ISoundService
    {
        private readonly SoundPlayer _soundPlayer;
        private readonly AudioClipsListConfig _audioClipsListConfig;
        private readonly GlobalEventProvider _globalEventProvider;

        public bool IsSoundOn => _soundPlayer.IsSoundOn;
        public bool IsMusicOn => _soundPlayer.IsMusicOn;

        public SoundService(SoundPlayer soundPlayer, ISoundConfigProvider soundConfigProvider,
            GlobalEventProvider globalEventProvider)
        {
            _soundPlayer = soundPlayer;
            _audioClipsListConfig = soundConfigProvider.AudioClipsListConfig;

            _globalEventProvider = globalEventProvider;
            _globalEventProvider.Invoke<SoundSwitchEvent, bool>(_soundPlayer.IsSoundOn);
            _globalEventProvider.Invoke<MusicSwitchEvent, bool>(_soundPlayer.IsMusicOn);
        }

        public void SwitchSoundState(bool isOn)
        {
            _soundPlayer.SwitchSoundState();
            _globalEventProvider.Invoke<SoundSwitchEvent, bool>(isOn);
        }

        public void SwitchMusicState(bool isOn)
        {
            _soundPlayer.SwitchMusicState();
            _globalEventProvider.Invoke<MusicSwitchEvent, bool>(isOn);
        }

        public void PlayGameBackgroundMusic() =>
            PlayMusic(_audioClipsListConfig.GameBackgroundMusic);

        public void PlayBallJumpSound() =>
            PlaySound(_audioClipsListConfig.BallJump);

        public void PlayBallPreMergeSound() =>
            PlaySound(_audioClipsListConfig.BallPreMerge);

        public void PlayBallMergeSound() =>
            PlaySound(_audioClipsListConfig.BallMerge);

        public void PlayPrizeHitSound() =>
            PlaySound(_audioClipsListConfig.PrizeHit);

        public void PlayAddCoinSound() =>
            PlaySound(_audioClipsListConfig.AddCoin);

        public void PlayButtonClickSound() =>
            PlaySound(_audioClipsListConfig.ButtonClick);

        public void PlayUpgradeButtonClickSound() =>
            PlaySound(_audioClipsListConfig.UpgradeButtonClick);

        public void PlayRewardSound() =>
            PlaySound(_audioClipsListConfig.Reward);

        public void PlayOneShot(AudioClipConfig clipConfig) =>
            _soundPlayer.PlaySoundAndCreateAudioSource(clipConfig);

        public void PlayWinSound()
        {
        }

        private void PlayMusic(AudioClipConfig musicConfig) => _soundPlayer.PlayMusic(musicConfig);
        private void PlaySound(AudioClipConfig soundConfig) => _soundPlayer.PlaySound(soundConfig);
    }
}