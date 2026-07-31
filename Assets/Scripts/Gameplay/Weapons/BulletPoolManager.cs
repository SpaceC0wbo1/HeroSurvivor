namespace HeroSurvivor.Gameplay.Weapons
{
    using System.Collections.Generic;
    using UnityEngine;

    public class BulletPoolManager : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public int poolSize = 30;
        public Transform spawnBulletTarget;

        private List<GameObject> pooledObjects = new List<GameObject>();

        void Start()
        {
            for (int i = 0; i < poolSize; i++)
            {
                CreateNewBullet();
            }
        }

        private GameObject CreateNewBullet()
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnBulletTarget);
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