namespace HeroSurvivor.Gameplay.Enemies
{
    using HeroSurvivor.Gameplay.Pursuit;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EnemySpawner : MonoBehaviour
    {
        [Header("Prefabs & Container")]
        public GameObject[] enemyPrefabs;
        public Transform poolContainer;

        [Header("3D Top-Down Spawn Settings")]
        [SerializeField] private Camera targetCamera;
        [SerializeField] private float viewportMargin = 0.15f;
        [SerializeField] private float groundY = 0f;

        [Header("Wave Settings")]
        [SerializeField] private float spawnDelay = 0.5f;
        [SerializeField] private int poolSize = 10;
        [SerializeField] private int enemiesPerWave = 5;

        private Queue<GameObject> enemyPool = new Queue<GameObject>();
        private WaitForSeconds cachedSpawnDelay;
        private Plane groundPlane;

        public bool IsSpawning { get; private set; }

        private void Awake()
        {
            cachedSpawnDelay = new WaitForSeconds(spawnDelay);

            if (targetCamera == null)
            {
                targetCamera = Camera.main;
            }

            groundPlane = new Plane(Vector3.up, new Vector3(0f, groundY, 0f));
        }

        private void Start()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject enemy = InstantiateNewEnemy();
                if (enemy != null)
                {
                    enemyPool.Enqueue(enemy);
                }
            }
        }

        public Coroutine SpawnWave()
        {
            return StartCoroutine(SpawnWaveRoutine());
        }

        private IEnumerator SpawnWaveRoutine()
        {
            IsSpawning = true;

            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return cachedSpawnDelay;
            }

            enemiesPerWave += 1;
            IsSpawning = false;
        }

        private GameObject InstantiateNewEnemy()
        {
            if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            {
                Debug.LogError("No enemy prefabs have been added to EnemySpawner!");
                return null;
            }

            int randomTypeEnemyIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject enemy = Instantiate(enemyPrefabs[randomTypeEnemyIndex], poolContainer);
            enemy.gameObject.SetActive(false);
            return enemy;
        }

        public GameObject GetPooledEnemy()
        {
            while (enemyPool.Count > 0)
            {
                GameObject enemy = enemyPool.Dequeue();

                if (enemy != null && !enemy.gameObject.activeInHierarchy)
                {
                    return enemy;
                }
            }
            return InstantiateNewEnemy();
        }

        public void ReturnToPool(GameObject enemy)
        {
            enemy.gameObject.SetActive(false);
            enemyPool.Enqueue(enemy);
        }

        private void SpawnEnemy()
        {
            GameObject spawnedEnemy = GetPooledEnemy();

            if (spawnedEnemy != null)
            {
                Vector3 spawnPosition = GetRandomOffScreenWorldPosition();

                spawnedEnemy.transform.position = spawnPosition;
                spawnedEnemy.transform.rotation = Quaternion.identity;
                spawnedEnemy.gameObject.SetActive(true);
            }
        }

        private Vector3 GetRandomOffScreenWorldPosition()
        {
            if (targetCamera == null)
            {
                return transform.position + Vector3.forward * 10f;
            }

            Vector2 viewportPoint = Vector2.zero;
            int side = Random.Range(0, 4);

            switch (side)
            {
                case 0:
                    viewportPoint = new Vector2(Random.Range(-viewportMargin, 1f + viewportMargin), 1f + viewportMargin);
                    break;
                case 1:
                    viewportPoint = new Vector2(Random.Range(-viewportMargin, 1f + viewportMargin), -viewportMargin);
                    break;
                case 2:
                    viewportPoint = new Vector2(-viewportMargin, Random.Range(-viewportMargin, 1f + viewportMargin));
                    break;
                case 3:
                    viewportPoint = new Vector2(1f + viewportMargin, Random.Range(-viewportMargin, 1f + viewportMargin));
                    break;
            }

            Ray ray = targetCamera.ViewportPointToRay(new Vector3(viewportPoint.x, viewportPoint.y, 0f));

            if (groundPlane.Raycast(ray, out float enterDistance))
            {
                return ray.GetPoint(enterDistance);
            }

            Vector2 randomCircle = Random.insideUnitCircle.normalized * 20f;
            return new Vector3(transform.position.x + randomCircle.x, groundY, transform.position.z + randomCircle.y);
        }
    }
}