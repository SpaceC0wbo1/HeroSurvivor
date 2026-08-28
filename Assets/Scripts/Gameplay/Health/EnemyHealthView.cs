using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using HeroSurvivor.Gameplay.Interfaces;

namespace HeroSurvivor.Gameplay.Health
{
    public class EnemyHealthView : MonoBehaviour, IDamageable, IHitFeedback
    {
        [SerializeField] private float _timeToDeactivate = 0.08f;
        [SerializeField] private CharacterConfig _characterConfig;


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
            DeactivateAsync(_timeToDeactivate, destroyCancellationToken).Forget();
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