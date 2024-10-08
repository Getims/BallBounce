﻿using Main.Scripts.Data.Services;
using Main.Scripts.Infrastructure.Providers.Events;
using TMPro;
using UnityEngine;
using Zenject;

namespace Main.Scripts.UI.Common
{
    public class LevelTracker : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _valueTMP;

        private GlobalEventProvider _globalEventProvider;
        private IProgressDataService _progressDataService;

        [Inject]
        public void Construct(IProgressDataService progressDataService, GlobalEventProvider globalEventProvider)
        {
            _progressDataService = progressDataService;
            _globalEventProvider = globalEventProvider;
            _globalEventProvider.AddListener<LevelSwitchEvent, int>(UpdateInfo);
        }

        public void UpdateInfo()
        {
            if (_progressDataService == null)
                return;

            UpdateInfo(_progressDataService.CurrentLevel);
        }

        private void Start() =>
            UpdateInfo();

        private void OnDestroy() =>
            _globalEventProvider?.RemoveListener<LevelSwitchEvent, int>(UpdateInfo);

        private void UpdateInfo(int levelNumber) =>
            _valueTMP.text = $"Level {levelNumber + 1}";
    }
}