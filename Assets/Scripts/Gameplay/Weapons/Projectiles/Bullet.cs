namespace HeroSurvivor.Gameplay.Weapons.Projectiles
{
    using HeroSurvivor.Gameplay.Enemies;
    using UnityEngine;

    public class Bullet : MonoBehaviour
    {
        public float bulletSpeed;
        public int bulletLiveTime;

        void OnEnable()
        {
            Invoke("Deactivate", bulletLiveTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
            {
                enemy.TakeDamage(12);
                gameObject.SetActive(false);
                CancelInvoke("Deactivate");
            }
        }

        void Deactivate()
        {
            gameObject.SetActive(false);
        }

        void Update()
        {
            transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        }
    }

}