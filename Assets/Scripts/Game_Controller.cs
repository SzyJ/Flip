using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Game_Controller : MonoBehaviour
{
    public TextMeshProUGUI summaryText;
    public Slider progressBar;
    public float degradeSpeed = 10.0f;

    public float enemyKillValue = 25.0f;
    private float progress = 100.0f;
    private float timeSurvived = 0.0f;
    private int enemiesKilled = 0;
    private EnemySpawner spawner;

    private Canvas summaryCanvas;
    private Canvas UICanvas;

    private bool gameRunning = true;

    // Start is called before the first frame update
    void Start()
    {
        summaryCanvas = GameObject.Find("SummaryCanvas").GetComponent<Canvas>();
        UICanvas = GameObject.Find("HUDCanvas").GetComponent<Canvas>();
        spawner = GameObject.Find("Enemy_Spawner").GetComponent<EnemySpawner>();
        
        UICanvas.enabled = true;
        summaryCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            Application.Quit();
        }

        if (!gameRunning)
        {
            return;
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
        PrintGameSummary(false);
    }

    public void PlayerDied()
    {
        PrintGameSummary(true);
    }

    public void EnemyKilled()
    {
        if (!gameRunning)
        {
            return;
        }
        spawner.onKill();
        progress += enemyKillValue;
        if (progress > 100.0f)
        {
            progress = 100.0f;
        }
        ++enemiesKilled;
    }

    void PrintGameSummary(bool deathPen)
    {
        gameRunning = false;
        UICanvas.enabled = false;
        summaryCanvas.enabled = true;

        string template = "Enemies Killed: {0}\nTime Survived: {1}\n";
        string deathTemplate = "Death Penalty: 70%\n";
        string totalTemplate = "\nTotal Score: {2}";

        int totalScore = enemiesKilled * 10 + Mathf.FloorToInt(timeSurvived) * 10;
        if (deathPen)
        {
            totalScore = Mathf.FloorToInt(totalScore * 0.3f);
        }

        summaryText.SetText(template + (deathPen ? deathTemplate : "") + totalTemplate, enemiesKilled, timeSurvived, totalScore);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
