using Main.Scripts.Configs.Balls;
using UnityEngine;

namespace Main.Scripts.GameLogic.Balls
{
    public class BallVisual : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _ballIcon;

        [SerializeField]
        private SpriteRenderer _ballOutline;

        [SerializeField]
        private TrailRenderer _trailRenderer;

        [SerializeField]
        private GameObject _upgradeParticles;

        private Vector3 _currentRotation;

        public void Initialize(BallConfig ballConfig)
        {
            _ballIcon.sprite = ballConfig.Sprite;
            _ballIcon.color = ballConfig.Color;
        }

        public void Show(bool showBall = true, bool showTrail = true)
        {
            if (showBall)
            {
                _ballIcon.enabled = true;
                _ballOutline.enabled = true;
            }

            if (showTrail)
                _trailRenderer.enabled = true;
        }

        public void Hide(bool hideBall = true, bool hideTrail = true)
        {
            if (hideBall)
            {
                _ballIcon.enabled = false;
                _ballOutline.enabled = false;
            }

            if (hideTrail)
                _trailRenderer.enabled = false;
        }

        public void ShowUpgradeParticles() =>
            _upgradeParticles.SetActive(true);

        public void SetRotationSpeed(Vector2 rotation) =>
            _currentRotation.z = -rotation.x * rotation.y;

        private void FixedUpdate() =>
            transform.Rotate(_currentRotation);
    }
}