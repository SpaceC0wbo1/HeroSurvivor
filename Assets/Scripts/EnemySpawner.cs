using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs & Points")]
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public Transform poolContainer;

    [Header("Settings")]
    public float spawnInterval = 5f;
    public float spawnDelay = 0.5f;
    public int poolSize = 10;
    public int enemiesPerWave = 5;

    private List<GameObject> enemyPool = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewEnemyInPool();
        }

        StartCoroutine(SpawnWaveRoutine());
    }

    private GameObject CreateNewEnemyInPool()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("No enemy prefabs have been added to EnemySpawner!");
            return null;
        }

        int randomTypeEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate(enemyPrefabs[randomTypeEnemyIndex], poolContainer);
        enemy.SetActive(false);
        enemyPool.Add(enemy);
        return enemy;
    }

    public GameObject GetPooledEnemy()
    {
        foreach (GameObject obj in enemyPool)
        {
            if (obj != null && !obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return CreateNewEnemyInPool();
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        GameObject spawnedEnemy = GetPooledEnemy();

        if (spawnedEnemy != null)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform selectedPoint = spawnPoints[randomIndex];

            spawnedEnemy.transform.position = selectedPoint.position;
            spawnedEnemy.transform.rotation = selectedPoint.rotation;
            spawnedEnemy.SetActive(true);
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
                yield return new WaitForSeconds(spawnDelay);
            }

            yield return new WaitForSeconds(spawnInterval);

            enemiesPerWave += 1;
            currentWave++;
        }
    }
}