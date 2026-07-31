namespace HeroSurvivor.Gameplay.Enemies
{
    using UnityEngine;

    public class TankEnemy : BaseEnemy
    {
        public override void Start()
        {
            enemyName = "Tank Enemy";
            base.Start();
        }

        public override void TakeDamage(int damageAmount)
        {
            int reducedDamage = damageAmount / 2;
            base.TakeDamage(reducedDamage);

        }
    }
}

