namespace HeroSurvivor.Gameplay.Enemies
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
