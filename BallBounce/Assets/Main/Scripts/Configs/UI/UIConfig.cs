using System;
using System.Collections.Generic;
using Main.Scripts.Configs.Core;
using Main.Scripts.UI.Base;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.UI
{
    [Serializable]
    public class UIConfig : ScriptableConfig
    {
        [SerializeField, Required, AssetsOnly]
        private List<UIPanel> _prefabs = new List<UIPanel>();

        public List<UIPanel> Prefabs => _prefabs;

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.UI_CATEGORY;
#endif
    }
}