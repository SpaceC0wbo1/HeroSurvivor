using HeroSurvivor.Gameplay.Animation;
using UnityEngine;

namespace HeroSurvivor.Gameplay.Movement
{
    public class MovementBody : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private MovementModifier[] _modifiers;

        private IMovementAnimator _movementAnimator;

        private void Awake()
        {
            if (_rigidBody == null)
                _rigidBody = GetComponent<Rigidbody>();

            _movementAnimator = GetComponentInChildren<IMovementAnimator>();
        }

        private void FixedUpdate()
        {
            Vector3 velocity = Vector3.zero;

            for (int i = 0; i < _modifiers.Length; i++) 
            { 
                MovementModifier modifier = _modifiers[i];

                velocity = modifier.Modify(velocity);
            }

            _rigidBody.linearVelocity = velocity;

            _movementAnimator?.SetMovementSpeed(velocity.magnitude);
        }
    }
}
