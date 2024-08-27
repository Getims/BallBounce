using Main.Scripts.GameLogic.Sound;
using UnityEngine;
using Zenject;

namespace Main.Scripts.UI.Base
{
    public class UIClickPlaySound : MonoBehaviour
    {
        private ISoundService _soundService;

        [Inject]
        public void Construct(ISoundService soundService) =>
            _soundService = soundService;

        public void OnClick() =>
            _soundService?.PlayButtonClickSound();
    }
}