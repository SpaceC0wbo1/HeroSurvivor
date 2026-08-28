using JetBrains.Annotations;
using System;
using UnityEngine;

namespace HeroSurvivor.Gameplay.Health
{
    public class HealthModel
    {
        public event Action<int, int> HealthChanged;
        public event Action<Vector3> Damaged;
        public event Action<Vector3> Died;

        public int MaxHealth { get; }
        public int CurrentHealth { get; private set; }
        public bool isDead => CurrentHealth <= 0;

        public HealthModel(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        public void ApplyDamage(int amount, Vector3 direction)
        {
            if (isDead)
                return;

            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
            HealthChanged?.Invoke(CurrentHealth, MaxHealth);

            if (isDead)
            {
                Died?.Invoke(direction);
            }
            else
            {
                Damaged?.Invoke(direction);
            }
        }

        public void ResetHealth()
        {
            CurrentHealth = MaxHealth;
            HealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }
    } 
}
