using Cysharp.Threading.Tasks;
using HeroSurvivor.Gameplay.Animation;
using HeroSurvivor.Gameplay.Interfaces;
using HeroSurvivor.Gameplay.Movement;
using HeroSurvivor.Gameplay.Rotation;
using System;
using System.Threading;
using UnityEngine;


namespace HeroSurvivor.Gameplay.Health
{
    public class EnemyHealthView : MonoBehaviour, IDamageable, IHitFeedback
    {
        [SerializeField] private float _timeToDeactivate = 0.08f;
        [SerializeField] private CharacterConfig _characterConfig;
        [SerializeField] private EnemyAnimatorView _animatorView;
        [SerializeField] private Collider _collider;
        [SerializeField] private MovementBody _movementBody;
        [SerializeField] private RotationBody _rotationBody;
        [SerializeField] private MonoBehaviour _attackBehaviour;
        [SerializeField] private Rigidbody _rigidBody;

        private EnemyHealthController _controller;

        public event Action<Vector3> OnHit;
        public event Action OnDeactivated;

        public CharacterConfig Config => _characterConfig;

        public void Construct(EnemyHealthController controller)
        {
            _controller = controller;
        }

        public void TakeDamage(int amount, Vector3 direction)
        {
            _controller?.TakeDamage(amount, direction);
        }

        public void PlayDeath(Vector3 hitDirection)
        {
            float delay = _timeToDeactivate;

            if (_animatorView != null) 
            {
                delay = _animatorView.PlayDeath();
            }
            DeactivateAsync(delay + 1.0f, destroyCancellationToken).Forget();
        }

        public void SetActiveEnemy(bool isActive)
        {
            if (_collider != null) _collider.enabled = isActive;
            if(_movementBody != null) _movementBody.enabled = isActive;
            if(_attackBehaviour != null) _attackBehaviour.enabled = isActive;
            if(_rotationBody != null) _rotationBody.enabled = isActive;

            if (!isActive)
            {
                if (_rigidBody != null)
                {
                    _rigidBody.linearVelocity = Vector3.zero;
                    _rigidBody.angularVelocity = Vector3.zero;
                    _rigidBody.isKinematic = true;
                    _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
                }
            }
            else
            {
                if (_rigidBody != null)
                {
                    _rigidBody.isKinematic = false;
                    _rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                }
            }
        }

        public void PlayHitEffect (Vector3 hitDirection)
        {
            OnHit?.Invoke(hitDirection);
        }

        private async UniTaskVoid DeactivateAsync(float seconds, CancellationToken token)
        {
            if (seconds > 0)
                await UniTask.WaitForSeconds(seconds, ignoreTimeScale: false, cancellationToken: token);

            OnDeactivated?.Invoke();
        }
    }
}