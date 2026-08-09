namespace HeroSurvivor.Gameplay.Shooting
{
    using System.Collections.Generic;
    using UnityEngine;

    public class BulletPoolManager : MonoBehaviour
    {

        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _spawnBulletTarget;
        [SerializeField] private int _poolSize = 30;

        private List<GameObject> pooledObjects = new List<GameObject>();

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
            pooledObjects.Add(bullet);
            return bullet;
        }

        public GameObject GetPooledObject()
        {
            foreach (GameObject obj in pooledObjects)
            {
                if (!obj.activeInHierarchy)
                {
                    return obj;
                }
            }

            return CreateNewBullet();
        }
    }

}