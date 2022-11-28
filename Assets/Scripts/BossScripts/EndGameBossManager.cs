using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGameBossManager : MonoBehaviour
{
    public UnityEvent OnGameEnd;
    public TextMeshProUGUI endGameText;
    public bool isGameDone = false;

    [Header("Boss Prefabs")]
    public GameObject dashBossPrefab;
    public Transform dashTransform;
    public GameObject projectilePrefab;
    public Transform projetileTransform;
    public GameObject towerPrefab;
    public Transform towerTransform;

    public bool dashBossDead { get; set; } = false;
    public bool projectileBossDead { get; set; } = false;
    public bool towerBossDead { get; set; } = false;
    public bool GameStart { get; set; } = false;

    private float currentGameTime = 0;

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha9))
        //{
        //    dashBossDead = true;
        //    projectileBossDead = true;
        //    towerBossDead = true;
        //}

        if(!isGameDone)
        {
            currentGameTime += Time.deltaTime;
            Debug.Log(currentGameTime);
        }

        if (dashBossDead && projectileBossDead && towerBossDead)
            isGameDone = true;

        if (isGameDone)
        {
            OnGameEnd?.Invoke();
            Debug.Log(currentGameTime);
            TimeSpan timeSpan = TimeSpan.FromSeconds(currentGameTime);
            endGameText.text = "Final time " + timeSpan.ToString(@"hh\:mm\:ss\:fff");
        }
    }

    //private string ConvertMillisecondsToFormatTime(float value)
    //{
    //    return timeSpan.ToString(@"hh\:mm\:ss\:fff");
    //}

    public void ResetAllBosses()
    {
        if(!dashBossDead)
            Instantiate(dashBossPrefab, dashTransform.position, Quaternion.identity);
        if(!projectileBossDead)
            Instantiate(projectilePrefab, projetileTransform.position, Quaternion.identity);
        if(!towerBossDead)
            Instantiate(towerPrefab, towerTransform.position, Quaternion.identity);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
