namespace HeroSurvivor.Gameplay.Enemies.Types
{
    using UnityEngine;

    public class FastEnemy : BaseEnemy
    {
        public override void Start()
        {
            enemyName = "Fast Enemy";
            base.Start();

            if (agent != null)
            {
                agent.speed = 8;
            }

        }
    }

}
