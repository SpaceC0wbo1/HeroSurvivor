using UnityEngine;

namespace HeroSurvivor.Gameplay.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(int amount, Vector3 direction);
    }
}
