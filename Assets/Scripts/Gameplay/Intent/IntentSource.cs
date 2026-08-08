using UnityEngine;

namespace HeroSurvivor.Gameplay.Intent
{
    public abstract class IntentSource : MonoBehaviour
    {
        public abstract Vector2 Direction { get; }
        public abstract Vector3 AimPoint { get; }
        public abstract bool IsShooting { get; }
    }
}
