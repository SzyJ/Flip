 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] spawnPoints;

    public GameObject enemyPrefab;

    public float changeDelay = 1000;

    private float timeRemaining = 0;

    public static int score = 0;

    public int enemySpawnLimit = 5;

    private int enemiesSpawned = 0;

    private float cumulitiveTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        cumulitiveTime += delta;
        if (timeRemaining <= 0)
        {
            timeRemaining = changeDelay;

            int spawnPoint = Random.Range(0, spawnPoints.Length - 1);

            Instantiate(enemyPrefab, spawnPoints[spawnPoint].transform);
            ++enemiesSpawned;
        }

        if (enemiesSpawned < enemySpawnLimit)
        {
            timeRemaining -= delta * Mathf.Sqrt(cumulitiveTime * 0.1f);
        }
    }
    
    public void onKill()
    {
        --enemiesSpawned;
    }
}
