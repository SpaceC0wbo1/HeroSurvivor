using UnityEngine;

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

        private void OnEnable()
        {
            Launch();
        }

        public void Launch()
        {
            _rigidBody.linearVelocity = transform.forward * _bulletSpeed;
            Invoke("Deactivate", _bulletLiveTime);
        }

        public void SetPosition(Vector3 sourcePosition)
        {
            _sourcePosition = sourcePosition;
        }

        public void SetDamage(int sourceDamage)
        {
            _sourceDamage = sourceDamage;
        }

        public void SetTargetTag(string targetTag)
        {
            _targetTag = targetTag;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!string.IsNullOrEmpty(_targetTag) && !other.CompareTag(_targetTag))
                return;

            Health health = other.GetComponentInParent<Health>();

            if (health != null)
            {
                Vector3 direction = other.transform.position - _sourcePosition;
                direction.y = 0f;
                direction.Normalize();
                _rigidBody.linearVelocity = Vector3.zero;

                health.TakeDamage(_sourceDamage, direction);
                gameObject.SetActive(false);
                CancelInvoke("Deactivate");
            }
        }

        void Deactivate()
        {
            _rigidBody.linearVelocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}