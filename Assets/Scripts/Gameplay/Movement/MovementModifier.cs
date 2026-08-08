using UnityEngine;

namespace HeroSurvivor.Gameplay.Movement
{
    public abstract class MovementModifier : MonoBehaviour
    {
        public abstract Vector3 Modify(Vector3 velocity);
    }
}
