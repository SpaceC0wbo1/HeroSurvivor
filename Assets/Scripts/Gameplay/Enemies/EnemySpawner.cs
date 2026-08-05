namespace HeroSurvivor.Gameplay.Enemies
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using HeroSurvivor.Gameplay.Player;

    public class EnemySpawner : MonoBehaviour
    {
        [Header("Prefabs & Points")]
        public Enemy[] enemyPrefabs;
        public Transform[] spawnPoints;
        public Transform poolContainer;

        [Header("Settings")]
        [SerializeField] private float spawnDelay = 0.5f;
        [SerializeField] private int poolSize = 10;
        [SerializeField] private int enemiesPerWave = 5;

        private Queue<Enemy> enemyPool = new Queue<Enemy>();
        private HeroController cachedPlayer;

        private WaitForSeconds cachedSpawnDelay;

        public bool IsSpawning { get; private set; }

        private void Awake()
        {
            cachedSpawnDelay = new WaitForSeconds(spawnDelay);
            cachedPlayer = FindAnyObjectByType<HeroController>();
        }

        private void Start()
        {
            if (cachedPlayer == null)
            {
                Debug.LogError("HeroController not found on GameScene!");
            }

            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                Debug.LogError("SpawnPoints array is empty! Assign spawn points in the Inspector.");
            }

            for (int i = 0; i < poolSize; i++)
            {
                Enemy enemy = InstantiateNewEnemy();
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

            // Збільшуємо кількість ворогів для наступної хвилі
            enemiesPerWave += 1;
            IsSpawning = false;
        }

        private Enemy InstantiateNewEnemy()
        {
            if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            {
                Debug.LogError("No enemy prefabs have been added to EnemySpawner!");
                return null;
            }

            int randomTypeEnemyIndex = Random.Range(0, enemyPrefabs.Length);
            Enemy enemy = Instantiate(enemyPrefabs[randomTypeEnemyIndex], poolContainer);
            enemy.gameObject.SetActive(false);
            return enemy;
        }

        public Enemy GetPooledEnemy()
        {
            while (enemyPool.Count > 0)
            {
                Enemy enemy = enemyPool.Dequeue();

                if (enemy != null && !enemy.gameObject.activeInHierarchy)
                {
                    return enemy;
                }
            }
            return InstantiateNewEnemy();
        }

        public void ReturnToPool(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
            enemyPool.Enqueue(enemy);
        }

        private void SpawnEnemy()
        {
            if (spawnPoints == null || spawnPoints.Length == 0) return;

            Enemy enemyScript = GetPooledEnemy();

            if (enemyScript != null)
            {
                if (cachedPlayer != null)
                {
                    enemyScript.Init(cachedPlayer);
                }

                int randomIndex = Random.Range(0, spawnPoints.Length);
                Transform selectedPoint = spawnPoints[randomIndex];

                enemyScript.transform.position = selectedPoint.position;
                enemyScript.transform.rotation = selectedPoint.rotation;
                enemyScript.gameObject.SetActive(true);
            }
        }
    }
}