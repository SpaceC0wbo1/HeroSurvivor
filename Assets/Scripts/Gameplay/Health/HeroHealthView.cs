using HeroSurvivor.Gameplay.Interfaces;
using HeroSurvivor.Gameplay.Animation;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;


namespace HeroSurvivor.Gameplay.Health
{
    public class HeroHealthView : MonoBehaviour, IDamageable, IHitFeedback
    {
        [SerializeField] private float _timeToDestroy = 0.08f;
        [SerializeField] private HeroAnimatorView _animatorView;

        public event Action<Vector3> OnHit;

        private HeroHealthController _controller;

        public void Construct(HeroHealthController controller)
        {
            _controller = controller;
        }

        public void TakeDamage (int amount, Vector3 direction)
        {
            _controller?.TakeDamage(amount, direction);
        }

        public void PlayDeath(Vector3 hitDirection)
        {
            float delay = _timeToDestroy;

            if (_animatorView != null) 
            { 
                delay = _animatorView.PlayDeath(); 
            }
            DestroyAsync(delay, destroyCancellationToken).Forget();
        }

        public void PlayHitEffect(Vector3 hitDirection)
        {
            OnHit?.Invoke(hitDirection);
        }

        private async UniTaskVoid DestroyAsync(float seconds, CancellationToken token)
        {
            await UniTask.WaitForSeconds(seconds, ignoreTimeScale: true, cancellationToken: token);
            Destroy(gameObject);
        }
    }
}
