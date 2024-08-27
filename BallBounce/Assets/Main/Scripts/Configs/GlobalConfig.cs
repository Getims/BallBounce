using System;
using Main.Scripts.Configs.Core;
using Main.Scripts.Configs.Sounds;
using UnityEngine;

namespace Main.Scripts.Configs
{
    [Serializable]
    public class GlobalConfig : ScriptableConfig
    {
        [SerializeField]
        private bool _enableDebug = false;
        
        [SerializeField]
        private VolumeConfig _volumeConfig;

        [SerializeField]
        private AudioClipsListConfig _audioClipsListConfig;

        public VolumeConfig VolumeConfig => _volumeConfig;
        public AudioClipsListConfig AudioClipsListConfig => _audioClipsListConfig;
        public bool EnableDebug => _enableDebug;
    }
}