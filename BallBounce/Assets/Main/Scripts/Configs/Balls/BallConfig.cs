using Main.Scripts.Configs.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Balls
{
    public class BallConfig : ScriptableConfig
    {
        [SerializeField, PreviewField(ObjectFieldAlignment.Left, Height = 80)]
        private Sprite _sprite;

        [SerializeField]
        private Color _color = Color.white;

        public Sprite Sprite => _sprite;
        public Color Color => _color;

#if UNITY_EDITOR
        public override string GetConfigCategory() =>
            ConfigsCategories.BALL_CATEGORY;
#endif
    }
}