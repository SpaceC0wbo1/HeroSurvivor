using UnityEngine;
using System;

namespace HeroSurvivor.Gameplay.Combat
{
    public class HealthEnemy : Health
    {
        public static event Action<int> OnEnemyDied;

        public override void TakeDamage(int amount, Vector3 direction)
        {
            if (_currentHealth <= 0)
                return;

            base.TakeDamage(amount, direction);

            if (_currentHealth <= 0)
            {
                OnEnemyDied?.Invoke(1);
            }
        }
    }
}