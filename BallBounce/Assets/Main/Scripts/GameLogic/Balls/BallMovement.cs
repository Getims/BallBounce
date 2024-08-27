using System;
using Main.Scripts.Core.Utilities;
using Main.Scripts.GameLogic.Platforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Scripts.GameLogic.Balls
{
    public class BallMovement : MonoBehaviour
    {
        private readonly RaycastHit2D[] _colliderHits = new RaycastHit2D[1];

        [SerializeField]
        private LayerMask _collisionLayer;

        [SerializeField]
        private float _mass = 1f;

        [SerializeField]
        private float _bounciness = 0.5f;

        [SerializeField]
        private Vector2 _platformBounceForce = Vector2.one;

        [SerializeField]
        private float _spawnXSpeed = 1.25f;
        
        [SerializeField]
        private float _minXSpeed = 2;

        [SerializeField]
        private float _maxMoveSpeed = 15;

        [SerializeField]
        private float _spawnCorrectionForce = 0.5f;

        [SerializeField]
        private float _acceleration = 1;

        private Vector2 _currentVelocity;
        private Vector2 _gravity;
        private Vector3 _lastPosition;
        private int _stuckCounter = 0;
        private float _spawnCorrection;
        private float _radius = 0.5f;

        public event Action<bool> OnBallHit;
        public event Action OnStuck;

        public Vector2 CurrentVelocity => _currentVelocity;
        private float TimeStep => Time.fixedDeltaTime * _acceleration;

        public void Initialize()
        {
            _currentVelocity = new Vector2(Random.Range(-_spawnXSpeed, _spawnXSpeed), -8);
            _radius = 0.5f * transform.localScale.x;
            _gravity = new Vector2(0, -Mathf.Abs(Physics2D.gravity.y) * _mass);
            _spawnCorrection = _spawnCorrectionForce;
            _stuckCounter = 0;
        }

        public void Move()
        {
            _currentVelocity += _gravity * TimeStep;
            _currentVelocity = Vector2.ClampMagnitude(_currentVelocity, _maxMoveSpeed);

            Vector2 position = transform.localPosition;
            Vector2 direction = CalculateDirection(position, ref _currentVelocity);

            transform.localPosition = position + direction;
            CheckStuck();
        }

        private Vector2 CalculateDirection(Vector2 position, ref Vector2 velocity)
        {
            Vector2 direction = velocity * TimeStep;
            int hitsCount = CheckHit(position, velocity.normalized, direction.magnitude);

            RaycastHit2D hit = _colliderHits[0];
            if (hitsCount == 0 || hit.collider == null)
                return direction;

            bool hitJumpPlatform = HitJumpPlatform(hit.collider);

            Vector2 newVelocity = _mass * GetBounciness(hitJumpPlatform) * _spawnCorrection *
                                  Vector2.Reflect(velocity / _mass, hit.normal);
            newVelocity = FixXSpeed(newVelocity, _minXSpeed, _mass);
            direction = newVelocity * TimeStep;

            if (CheckHit(position + direction, Vector2.zero, 0) == 0)
            {
                velocity = newVelocity;
                _spawnCorrection = 1;
            }
            else
            {
                velocity = -velocity;
                direction = Vector2.zero;
            }

            OnBallHit?.Invoke(hitJumpPlatform);
            return direction;
        }

        private int CheckHit(Vector2 position, Vector2 direction, float distance) =>
            Physics2D.CircleCastNonAlloc(position, _radius, direction, _colliderHits, distance, _collisionLayer);

        private bool HitJumpPlatform(Collider2D hitCollider)
        {
            if (hitCollider.TryGetComponent(out Platform platform))
            {
                platform.BallHit();
                return platform.CanGiveMoney();
            }

            return false;
        }

        private float GetBounciness(bool hitJumpPlatform)
        {
            float bouncinessFix = (Time.fixedDeltaTime * _acceleration - Time.fixedDeltaTime) * 1.5f;
            float bounciness = _bounciness;

            if (hitJumpPlatform)
                bounciness = Random.Range(_platformBounceForce.x, _platformBounceForce.y);

            return bounciness - bouncinessFix;
        }

        private Vector2 FixXSpeed(Vector2 velocity, float minSpeed, float mass)
        {
            if (Mathf.Abs(velocity.x) > minSpeed)
                return velocity;

            if (Utils.IncludesInInterval(velocity.x, new Vector2(-0.1f, 0.1f)))
                velocity.x = Utils.RandomValue(-minSpeed, minSpeed) * mass;
            else
                velocity.x = Utils.Normalize(velocity.x) * minSpeed * mass;
            return velocity;
        }

        private void CheckStuck()
        {
            if (transform.localPosition == _lastPosition)
            {
                _stuckCounter++;
                if (_stuckCounter >= 5)
                    OnStuck?.Invoke();
            }
            else
                _stuckCounter = 0;

            _lastPosition = transform.localPosition;
        }
    }
}