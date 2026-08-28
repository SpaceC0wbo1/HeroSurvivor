using System;
using UnityEngine;
using Zenject;

namespace HeroSurvivor.Gameplay.Health
{
    public class HeroHealthController : IInitializable, IDisposable
    {
        private readonly HealthModel _model;
        private readonly HeroHealthView _view;
        private readonly SignalBus _signalBus;

        public HeroHealthController(HealthModel model, HeroHealthView view, SignalBus signalBus)
        {
            _model = model;
            _view = view;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _view.Construct(this);

            _model.Damaged += OnDamaged;
            _model.Died += OnDied;
            _model.HealthChanged += OnHealthChanged;

            _signalBus.Fire(new HeroHealthChangedSignal
            {
                CurrentHealth = _model.CurrentHealth,
                MaxHealth = _model.MaxHealth
            });
        }

        public void Dispose()
        {
            _model.Damaged -= OnDamaged;
            _model.Died -= OnDied;
            _model.HealthChanged -= OnHealthChanged;
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

        private void OnHealthChanged(int current, int max)
        {
            _signalBus.Fire(new HeroHealthChangedSignal
            {
                CurrentHealth = current,
                MaxHealth = max
            });
        }

        private void OnDied(Vector3 direction)
        {
            _view.PlayDeath(direction);

            _signalBus.Fire<HeroDiedSignal>();
            _signalBus.Fire(new AnyKilledSignal
            {
                Position = _view.transform.position,
                Direction = direction
            });
        }
    }
}