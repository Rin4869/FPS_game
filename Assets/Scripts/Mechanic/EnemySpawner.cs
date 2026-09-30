using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject enemyPrefab;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;


    [Header("Wave")]
    public int startingEnemies = 3;
    public int enemiesAddedPerWave = 1;
    public float spawnDelay = 0.5f;
    public float waveDelay = 2f;

    private int currentWave = 0;
    private int enemiesAlive = 0;

    private bool isSpawningWave = false;

    private bool waitingForNextWave = false;
    public int CurrentWave => currentWave;
    public int EnemiesAlive => enemiesAlive;

    void Start()
    {
        StartNextWave();
    }

    void Update()
    {
        if (enemiesAlive <= 0 &&
            !isSpawningWave &&
            !waitingForNextWave)
        {
            StartCoroutine(StartNextWaveAfterDelay());
        }
    }

        IEnumerator StartNextWaveAfterDelay()
    {
        waitingForNextWave = true;

        Debug.Log(
            "Next wave starting in " +
            waveDelay +
            " seconds..."
        );

        yield return new WaitForSeconds(waveDelay);

        waitingForNextWave = false;

        StartNextWave();
    }

    void StartNextWave()
    {
        if (isSpawningWave)
            return;

        currentWave++;

        int enemyCount =
            startingEnemies +
            (currentWave - 1) * enemiesAddedPerWave;

        Debug.Log(
            "Starting Wave " +
            currentWave +
            " - Enemies: " +
            enemyCount
        );

        StartCoroutine(SpawnWave(enemyCount));
    }

    IEnumerator SpawnWave(int enemyCount)
    {
        isSpawningWave = true;

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();

            yield return new WaitForSeconds(spawnDelay);
        }

        isSpawningWave = false;
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError(
                "Enemy Prefab is not assigned!"
            );

            return;
        }

        if (spawnPoints == null ||
            spawnPoints.Length == 0)
        {
            Debug.LogError(
                "No spawn points assigned!"
            );

            return;
        }

        Transform spawnPoint =
            spawnPoints[
                Random.Range(
                    0,
                    spawnPoints.Length
                )
            ];

        Instantiate(
            enemyPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        enemiesAlive++;
    }

    public void EnemyDefeated()
    {
        enemiesAlive--;

        Debug.Log(
            "Enemy defeated. Remaining: " +
            enemiesAlive
        );
    }
}