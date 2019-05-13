using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Controller : MonoBehaviour
{
    public Slider progressBar;
    public float degradeSpeed = 10.0f;

    public float enemyKillValue = 25.0f;
    private float progress = 100.0f;
    private float timeSurvived = 0.0f;
    private int enemiesKilled = 0;
    private EnemySpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.Find("Enemy_Spawner").GetComponent<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            Application.Quit();
        }

        float delta = Time.deltaTime;
        progress -= delta * degradeSpeed;
        timeSurvived += delta;
        if (progress <= 0)
        {
            GameEnd();
        }

        progressBar.value = progress;

    }

    void GameEnd()
    {
        progress = 100.0f;
    }

    public void PlayerDied()
    {

    }

    public void EnemyKilled()
    {
        spawner.onKill();
        progress += enemyKillValue;
        if (progress > 100.0f)
        {
            progress = 100.0f;
        }
        ++enemiesKilled;
    }
}
