using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Scripts.GameLogic.GameFlow
{
    public class GameZone : MonoBehaviour
    {
        [SerializeField]
        private List<SpriteRenderer> _helpers = new List<SpriteRenderer>();

        public float MinX => _position.x - _scale.x * .5f;
        public float MaxX => _position.x + _scale.x * .5f;
        public float MinY => _position.y - _scale.y * .5f;
        public float MaxY => _position.y + _scale.y * .5f;
        public float CenterX => _position.x;
        public float CenterY => _position.y;

        public float Width => _scale.x;
        public float Height => _scale.y;

        private Vector3 _position => transform.position;
        private Vector3 _scale => transform.lossyScale;

        public void Initialize()
        {
            foreach (SpriteRenderer helper in _helpers)
                helper.enabled = false;
        }

        private void Awake() => Initialize();

        private void Start()
        {
        }
    }
}