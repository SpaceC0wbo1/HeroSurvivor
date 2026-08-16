using HeroSurvivor.Gameplay.Combat;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace HeroSurvivor.Gameplay.Movement
{
    public class KnockbackModifier : MovementModifier
    {
        [Header("Setup")]
        [SerializeField] private Health _health;

        [Header("Knockback")]
        [SerializeField] private bool _enabled;
        [SerializeField] private float _force = 15f;
        [SerializeField] private float _damping = 80f;

        private Vector3 _currentImpulse;

        private void OnEnable()
        {
            _health.Damaged += OnHit;
            _health.Killed += OnHit;
        }

        private void OnDisable()
        {
            _health.Damaged -= OnHit;
            _health.Killed -= OnHit;
        }

        private void OnHit(Vector3 direction)
        {
            if(_enabled == false) 
                return;

            _currentImpulse = direction * _force;
        }

        public override Vector3 Modify (Vector3 velocity)
        {
            if (_enabled == false)
            {
                _currentImpulse = Vector3.zero;
                return velocity;
            }

            Vector3 result = velocity + _currentImpulse;
            _currentImpulse = Vector3.MoveTowards(_currentImpulse, Vector3.zero, _damping * Time.fixedDeltaTime);

            return result;
        }

    }
}
