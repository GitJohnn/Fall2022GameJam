using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class EndGameBossManager : MonoBehaviour
{
    public UnityEvent OnGameEnd;
    public TextMeshProUGUI endGameText;
    public static bool isGameDone = false;

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
        if (!GameStart)
            return;

        if(!isGameDone)
            currentGameTime += Time.deltaTime;

        if (dashBossDead && projectileBossDead && towerBossDead)
            isGameDone = true;

        if (isGameDone)
        {
            endGameText.text = "Final time " + ConvertMillisecondsToFormatTime(currentGameTime);
            OnGameEnd?.Invoke();
        }
    }

    private string ConvertMillisecondsToFormatTime(float value)
    {
        TimeSpan timeSpan = TimeSpan.FromMilliseconds(value);
        return timeSpan.ToString(@"\.hh\\:mm\\:ss\\:fff");
    }

    public void ResetAllBosses()
    {
        if(!dashBossDead)
            Instantiate(dashBossPrefab, dashTransform.position, Quaternion.identity);
        if(!projectileBossDead)
            Instantiate(projectilePrefab, projetileTransform.position, Quaternion.identity);
        if(!towerBossDead)
            Instantiate(towerPrefab, towerTransform.position, Quaternion.identity);
    }
}
