using System.Collections.Generic;
using UnityEngine;

namespace HeroSurvivor.Gameplay.Combat
{
    public class BulletPoolManager : MonoBehaviour
    {

        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _spawnBulletTarget;
        [SerializeField] private int _poolSize = 30;

        private Queue<GameObject> _pool = new Queue<GameObject>();

        void Start()
        {
            for (int i = 0; i < _poolSize; i++)
            {
                CreateNewBullet();
            }
        }

        private GameObject CreateNewBullet()
        {
            GameObject bullet = Instantiate(_bulletPrefab, _spawnBulletTarget);
            bullet.SetActive(false);
            _pool.Enqueue(bullet);
            return bullet;
        }

        public GameObject GetPooledObject()
        {
            int checkedCount = 0;
            int initialCount = _pool.Count;

            while (checkedCount < initialCount) 
            { 
                GameObject bullet = _pool.Dequeue();
                _pool.Enqueue(bullet);
                checkedCount++;

                if (!bullet.activeInHierarchy)
                {
                    return bullet;
                }
            }

            return CreateNewBullet();
        }
    }

}