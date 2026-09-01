using System;
using UnityEngine;
using Zenject;

namespace HeroSurvivor.Gameplay.Health
{
    public class EnemyHealthController : IDisposable
    {
        private readonly HealthModel _model;
        private readonly EnemyHealthView _view;
        private readonly SignalBus _signalBus;
        private readonly Action<GameObject> _returnToPoolAction;

        public EnemyHealthController(
            HealthModel model,
            EnemyHealthView view,
            SignalBus signalBus,
            Action<GameObject> returnToPoolAction)
        {
            _model = model;
            _view = view;
            _signalBus = signalBus;
            _returnToPoolAction = returnToPoolAction;

            _view.Construct(this);

            _model.Damaged += OnDamaged;
            _model.Died += OnDied;
            _view.OnDeactivated += ReturnToPool;
        }

        public void ResetState()
        {
            _model.ResetHealth();
            _view.SetActiveEnemy(true);
        }

        public void TakeDamage(int amount, Vector3 direction)
        {
            Debug.Log($"Enemy {_view.gameObject.name} took {amount} damage. HP before: {_model.CurrentHealth} / Max: {_model.MaxHealth}");
            _model.ApplyDamage(amount, direction);
        }

        private void OnDamaged(Vector3 direction)
        {
            _view.PlayHitEffect(direction);

            _signalBus.Fire(new AnyDamagedSignal
            {
                HitPoint = _view.transform.position,
                Direction = direction
            });
        }

        private void OnDied(Vector3 direction)
        {
            _view.PlayHitEffect(direction);
            _view.PlayDeath(direction);
            _view.SetActiveEnemy(false);

            _signalBus.Fire(new EnemyDiedSignal
            {
                RewardPoints = 1
            });

            _signalBus.Fire(new AnyKilledSignal
            {
                Position = _view.transform.position,
                Direction = direction
            });
        }

        private void ReturnToPool()
        {
            _returnToPoolAction?.Invoke(_view.gameObject);
        }

        public void Dispose()
        {
            _model.Damaged -= OnDamaged;
            _model.Died -= OnDied;
            _view.OnDeactivated -= ReturnToPool;
        }
    }
}