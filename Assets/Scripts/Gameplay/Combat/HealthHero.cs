using UnityEngine;
using System;

namespace HeroSurvivor.Gameplay.Combat
{
    public class HealthHero : Health
    {
        public static event Action <int,int> OnHealthChanged;

        public static Action OnHeroDied;

        private void Start()
        {
            OnHealthChanged?.Invoke(_currentHealth, MaxHealth);
        }

        public override void TakeDamage(int amount, Vector3 direction)
        {
            if (_currentHealth <= 0)
                return;

            base.TakeDamage(amount, direction);

            OnHealthChanged?.Invoke(Mathf.Max(0, _currentHealth), MaxHealth);

            if (_currentHealth <= 0)
            {
                OnHeroDied?.Invoke();
            }
        }
    }
}
