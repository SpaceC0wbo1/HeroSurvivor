using UnityEngine;

namespace HeroSurvivor.Gameplay.Rotation
{
    public class RotationBody : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private RotationModifier[] _modifiers;

        private void FixedUpdate()
        {
            Quaternion rotation = _rigidBody.rotation;

            for (int i = 0; i < _modifiers.Length; i++) 
            { 
                RotationModifier modifier = _modifiers[i]; 

                if (modifier == null)
                    continue;

                if (modifier.enabled == false)
                    continue;

                rotation = modifier.Modify(rotation);
            }

            _rigidBody.MoveRotation(rotation);
        }
    }
}
