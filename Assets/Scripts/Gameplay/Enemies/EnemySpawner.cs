namespace HeroSurvivor.Gameplay.Enemies
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using HeroSurvivor.Gameplay.Player;

    public class EnemySpawner : MonoBehaviour
    {
        [Header("Prefabs & Points")]
        public BaseEnemy[] enemyPrefabs;
        public Transform[] spawnPoints;
        public Transform poolContainer;

        [Header("Settings")]
        [SerializeField] private float spawnInterval = 5f;
        [SerializeField] private float spawnDelay = 0.5f;
        [SerializeField] private int poolSize = 10;
        [SerializeField] private int enemiesPerWave = 5;


        private Queue<BaseEnemy> enemyPool = new Queue<BaseEnemy>();
        private HeroController cachedPlayer;

        private WaitForSeconds cachedSpawnDelay;
        private WaitForSeconds cachedSpawnInterval;

        private void Awake()
        {
            cachedSpawnDelay = new WaitForSeconds(spawnDelay);
            cachedSpawnInterval = new WaitForSeconds(spawnInterval);
        }

        private void Start()
        {
            cachedPlayer = FindAnyObjectByType<HeroController>();

            if (cachedPlayer == null)
            {
                Debug.LogError("HeroController not found on GameScene!");
            }

            for (int i = 0; i < poolSize; i++)
            {
                CreateNewEnemyInPool();
            }

            StartCoroutine(SpawnWaveRoutine());
        }

        private BaseEnemy CreateNewEnemyInPool()
        {
            if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            {
                Debug.LogError("No enemy prefabs have been added to EnemySpawner!");
                return null;
            }

            int randomTypeEnemyIndex = Random.Range(0, enemyPrefabs.Length);
            BaseEnemy enemy = Instantiate(enemyPrefabs[randomTypeEnemyIndex], poolContainer);
            enemy.gameObject.SetActive(false);
            enemyPool.Enqueue(enemy);
            return enemy;
        }

        public BaseEnemy GetPooledEnemy()
        {
            if (enemyPool.Count > 0)
            {
                BaseEnemy enemy = enemyPool.Dequeue();

                if (enemy != null && !enemy.gameObject.activeInHierarchy)
                {
                    return enemy;
                }
            }

            return CreateNewEnemyInPool();
        }

        public void ReturnToPool(BaseEnemy enemy)
        {
            enemy.gameObject.SetActive(false);
            enemyPool.Enqueue(enemy);
        }

        private void SpawnEnemy()
        {
            if (spawnPoints.Length == 0) return;

            BaseEnemy enemyScript = GetPooledEnemy();

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

        private IEnumerator SpawnWaveRoutine()
        {
            int currentWave = 1;

            while (true)
            {
                Debug.Log($"Wave {currentWave} started!");

                for (int i = 0; i < enemiesPerWave; i++)
                {
                    SpawnEnemy();
                    yield return cachedSpawnDelay; // Перевикористовуємо кешований затримач
                }

                yield return cachedSpawnInterval; // Перевикористовуємо кешований затримач

                enemiesPerWave += 1;
                currentWave++;
            }
        }
    }
}