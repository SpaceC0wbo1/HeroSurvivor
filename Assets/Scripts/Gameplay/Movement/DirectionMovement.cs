using HeroSurvivor.Gameplay.Intent;
using UnityEngine;

namespace HeroSurvivor.Gameplay.Movement
{
    public class DirectionMovement : MovementModifier
    {
        [SerializeField] private IntentSource _intent;
        [SerializeField] private CharacterConfig _characterConfig;

        public override Vector3 Modify(Vector3 velocity)
        {
            Vector2 direction = _intent.Direction;
            return velocity + new Vector3(direction.x, 0f, direction.y) * _characterConfig.speedMovement;
        }
    }
}
