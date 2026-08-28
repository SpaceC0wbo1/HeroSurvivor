using UnityEngine;
using HeroSurvivor.Gameplay.Interfaces;

namespace HeroSurvivor.Gameplay.Combat
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private float _bulletLiveTime = 3f;
        [SerializeField] private float _bulletSpeed = 20f;

        private int _sourceDamage;
        private Vector3 _sourcePosition;
        private string _targetTag;
        private float _lifeTimer;

        private void OnEnable()
        {
            _lifeTimer = _bulletLiveTime;
            _rigidBody.linearVelocity = transform.forward * _bulletSpeed;
        }

        private void Update()
        {
            _lifeTimer -= Time.deltaTime;
            if (_lifeTimer <= 0) 
            { 
                Deactivate(); 
            }
        }

        public void SetPosition(Vector3 sourcePosition) => _sourcePosition = sourcePosition;

        public void SetDamage(int sourceDamage) => _sourceDamage = sourceDamage;

        public void SetTargetTag(string targetTag) => _targetTag = targetTag;


        private void OnTriggerEnter(Collider other)
        {
            if (!string.IsNullOrEmpty(_targetTag) && !other.CompareTag(_targetTag))
                return;

            if (other.GetComponentInParent<IDamageable>() is IDamageable damageable)
            { 
                Vector3 direction = other.transform.position - _sourcePosition;
                direction.y = 0f;
                direction.Normalize();
                
                damageable.TakeDamage(_sourceDamage, direction);
                Deactivate();
            }
        }

        void Deactivate()
        {
            _rigidBody.linearVelocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}