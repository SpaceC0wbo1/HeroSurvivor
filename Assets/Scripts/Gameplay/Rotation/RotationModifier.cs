using UnityEngine;

namespace HeroSurvivor.Gameplay.Rotation
{
    public abstract class RotationModifier : MonoBehaviour
    {
        public abstract Quaternion Modify(Quaternion rotation);
    }
}
