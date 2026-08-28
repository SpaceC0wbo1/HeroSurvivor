using UnityEngine;

namespace HeroSurvivor.Gameplay.Health
{
        public struct EnemyDiedSignal
        {
            public int RewardPoints;
        }

        public struct HeroHealthChangedSignal
        {
            public int CurrentHealth;
            public int MaxHealth;
        }

    public struct HeroDiedSignal { }

    public struct WeaponFiredSignal { }

    public struct  AnyDamagedSignal
    {
        public Vector3 HitPoint;
        public Vector3 Direction;
    }

    public struct AnyKilledSignal
    {
        public Vector3 Position;
        public Vector3 Direction;
    }
}
