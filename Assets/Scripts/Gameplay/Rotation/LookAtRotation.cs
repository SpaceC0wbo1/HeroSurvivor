using UnityEngine;
using HeroSurvivor.Gameplay.Intent;

namespace HeroSurvivor.Gameplay.Rotation
{
    public class LookAtRotation : RotationModifier
    {
        private const float MIN_AIM_DISTANCE_SQR = 0.01f;

        [SerializeField] private IntentSource _intent;

        public override Quaternion Modify(Quaternion rotation)
        {
            Vector3 toAim = _intent.AimPoint - transform.position;
            toAim.y = 0f;

            if (toAim.sqrMagnitude < MIN_AIM_DISTANCE_SQR)
                return rotation;

            return Quaternion.LookRotation(toAim);
        }
    }
}
