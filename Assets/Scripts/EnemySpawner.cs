 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] spawnPoints;

    public GameObject enemyPrefab;

    public float changeDelay = 1000;

    private float timeRemaining = 0;
    private GameObject enemyObject = null;

    public static int score = 0; 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            Application.Quit();
        }

        if (timeRemaining <= 0)
        {
            if (enemyObject != null)
            {
                Destroy(enemyObject);
            }
            timeRemaining = changeDelay;

            int spawnPoint = Random.Range(0, spawnPoints.Length - 1);

            enemyObject = (GameObject) Instantiate(enemyPrefab, spawnPoints[spawnPoint].transform);
        }

        timeRemaining -= Time.deltaTime;
    }

    public int getScore()
    {
        return score;
    }

    public void onKill()
    {
        score += 100;

        Debug.Log("Score is: " + score);
        timeRemaining = 0;
    }
}
