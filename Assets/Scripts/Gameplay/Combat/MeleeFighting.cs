using HeroSurvivor.Gameplay.Intent;
using HeroSurvivor.Gameplay.Interfaces;
using HeroSurvivor.Gameplay.Animation;
using UnityEngine;

namespace HeroSurvivor.Gameplay.Combat
{
    public class MeleeFighting : MonoBehaviour
    {
        [SerializeField] private CharacterConfig _characterConfig;
        [SerializeField] private IntentSource _intent;
        [SerializeField] private EnemyAnimatorView _animatorView;

        private float nextAttackTime;

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (Time.time < nextAttackTime)
                return;

            if (other.GetComponentInParent<IDamageable>() is IDamageable damageable)
            {
                {
                    Vector3 direction = other.transform.position - transform.position;
                    direction.y = 0f;
                    direction.Normalize();

                    damageable.TakeDamage(_characterConfig.damage, direction);
                    nextAttackTime = Time.time + _characterConfig.attackInterval;
                    _animatorView.PlayAttack();
                }
            }
        }
    }
}