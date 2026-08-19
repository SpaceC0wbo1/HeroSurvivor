using HeroSurvivor.Gameplay.Intent;
using UnityEngine;

namespace HeroSurvivor.Gameplay.Combat
{
    public class MeleeFighting : MonoBehaviour
    {
        [SerializeField] private CharacterConfig _characterConfig;
        [SerializeField] private IntentSource _intent;

        private float nextAttackTime;

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (Time.time < nextAttackTime)
                return;

            Health health = other.GetComponentInParent<Health>();

            if (health != null)
            {
                Vector3 direction = other.transform.position - transform.position;
                direction.y = 0f;
                direction.Normalize();

                health.TakeDamage(_characterConfig.damage, direction);
                nextAttackTime = Time.time + _characterConfig.attackInterval;
            }
        }
    }
}