using UnityEngine;
using HeroSurvivor.Gameplay.Intent;

namespace HeroSurvivor.Gameplay.Pursuit
{
    public class PursuitAI : IntentSource
    {
        private const float MIN_TARGET_DISTANCE_QR = 0.0001f;

        [SerializeField] private CharacterConfig _characterConfig;
        private GameObject _target;

        private void Awake()
        {
            _target = GameObject.FindWithTag(_characterConfig.targetTag);
        }

        public override Vector2 Direction
        {
            get
            {
                if (_target == null)
                    return Vector2.zero;

                Vector3 delta = _target.transform.position - transform.position;
                delta.y = 0f;

                if (delta.sqrMagnitude < MIN_TARGET_DISTANCE_QR)
                    return Vector2.zero;

                Vector3 normalized = delta.normalized;

                return new Vector2(normalized.x, normalized.z);
            }
        }

        public override Vector3 AimPoint
        {
            get
            {
                if (_target == null)
                    return transform.position;

                return _target.transform.position;
            }
        }
        public override bool IsShooting => false;

    }
}
