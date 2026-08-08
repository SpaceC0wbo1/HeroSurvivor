using UnityEngine;

namespace HeroSurvivor.Gameplay.Movement
{
    public class MovementBody : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private MovementModifier[] _modifiers;

        private void FixedUpdate()
        {
            Vector3 velocity = Vector3.zero;

            for (int i = 0; i < _modifiers.Length; i++) 
            { 
                MovementModifier modifier = _modifiers[i];

                velocity = modifier.Modify(velocity);
            }

            _rigidBody.linearVelocity = velocity;
        }
    }
}
