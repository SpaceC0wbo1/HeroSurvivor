namespace HeroSurvivor.Gameplay.Shooting
{
    using HeroSurvivor.Gameplay.Enemies;
    using UnityEngine;

    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private float _bulletLiveTime = 3f;
        [SerializeField] private float _bulletSpeed = 20f;

        private Vector3 _sourcePosition;

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

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(12);
                _rigidBody.linearVelocity = Vector3.zero;
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