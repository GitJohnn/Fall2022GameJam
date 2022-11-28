using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TowerBossScript : MonoBehaviour
{
    public TowerBossMovement towerBossMovement;
    public TowerBossAttack towerBossAttack;
    public EnemyHealth towerBossHealth;
    public Slider towerBossHealthSlider;
    public bool startOnAwake = false;

    [SerializeField]public UnityEvent OnTowerBossDead;

    public static bool isBossDead = false;

    public Transform playerTransform;
    public Transform movePlayerPos;
    public float timeBeforeNextPhase = 6;

    [Header("Phase 1 Health")]
    [SerializeField] List<ObstacleHealth> phase1Health;
    private bool[] phase1BoolArray;

    [Header("Phase 2 Health")]
    [SerializeField] List<ObstacleHealth> phase2Health;

    private bool phase1Complete = false;
    private int phase1HealthAmount;
    private int currentPhase1Amount = 0;
    [SerializeField] UnityEvent OnPhase1Complete;

    private bool phase2Complete = false;
    private bool phase2Started = false;
    private int phase2HealthAmount;
    private int currentPhase2Amount = 0;
    [SerializeField] UnityEvent OnStartPhase2;
    [SerializeField] UnityEvent OnPhase2Complete;

    [SerializeField] UnityEvent onStartMovePlayer;
    [SerializeField] UnityEvent onEndMovePlayer;

    [SerializeField] UnityEvent OnScreenFadeAfterDeath;
    private bool isMovingPlayer = false;
    private float timeToMovePlayer = 2;
    private float currentTimeToMovePlayer = 0;
    private bool runOnce = false;

    private void Awake()
    {
        if (!playerTransform)
            playerTransform = GameObject.FindObjectOfType<PlayerMovement>().transform;
        phase1HealthAmount = phase1Health.Count;        
        phase2HealthAmount = phase2Health.Count;
        //Debug.Log(phase1HealthAmount + " " + phase2HealthAmount);
        if (startOnAwake)
            StartBossFight();
    }

    private void Update()
    {
        if (isBossDead && !runOnce)
        {
            runOnce = true;
            FadeAnimationScript.OnFaded += (OnScreenFadeAfterDeath.Invoke);
            if (MusicManager.Instance) MusicManager.Instance.SwapTrack(); 
            StartCoroutine(AfterFadeUnsubscribe());
            return;
        }

        towerBossHealthSlider.value = towerBossHealth.GetHealthPercentage();

        //Check boss health drops to second phase limit and player has destroyed all walls
        if(phase1Complete && !phase2Started && towerBossHealth.GetHealthPercentage() <= 0.5f)
        {
            StartEarlyPhase2();
        }
        
        if (!isMovingPlayer)
            return;

        if(currentTimeToMovePlayer < timeToMovePlayer)
        {
            currentTimeToMovePlayer += Time.deltaTime;
            MovePlayerBeforeNextPhaseStart();
        }
    }

    public void StartBossFight()
    {
        isBossDead = false;
        towerBossMovement.StopMovement = false;
        towerBossAttack.StopAttacking = false;
    }

    public void CheckPhase1HealthObstacles()
    {
        currentPhase1Amount++;

        if (currentPhase1Amount < phase1HealthAmount)
            return;

        phase1Complete = true;
        OnPhase1Complete?.Invoke();
        StartCoroutine(TimeBeforeNextPhase(timeBeforeNextPhase));
    }

    public void StartEarlyPhase2()
    {
        Debug.Log("Early phase 2");
        StartCoroutine(EarlyPhase2StartCoroutine());
    }

    public void CheckPhase2HealthObstacles()
    {
        currentPhase2Amount++;

        if (currentPhase2Amount < phase2HealthAmount)
            return;

        //Debug.Log("complete phase 2");
        phase2Complete = true;
        OnPhase2Complete?.Invoke();
    }

    public void MovePlayerBeforeNextPhaseStart()
    {
        //Debug.Log("moving player");
        playerTransform.position = Vector2.Lerp(playerTransform.position, movePlayerPos.position, currentTimeToMovePlayer / timeToMovePlayer);
    }

    IEnumerator TimeBeforeNextPhase(float value)
    {
        float currentWaitTime = 0;
        while (currentWaitTime < value)
        {
            currentWaitTime += Time.deltaTime;
            yield return null;
        }
        //yield return new WaitForSeconds(value);
        isMovingPlayer = true;
        onStartMovePlayer?.Invoke();
        yield return new WaitForSeconds(timeToMovePlayer);
        isMovingPlayer = false;
        onEndMovePlayer?.Invoke();
        OnStartPhase2?.Invoke();
        phase2Started = true;
    }

    IEnumerator EarlyPhase2StartCoroutine()
    {
        isMovingPlayer = true;
        onStartMovePlayer?.Invoke();
        yield return new WaitForSeconds(timeToMovePlayer);
        isMovingPlayer = false;
        onEndMovePlayer?.Invoke();
        OnStartPhase2?.Invoke();
        phase2Started = true;
    }

    public void TriggerBossDead()
    {
        isBossDead = true;
        StartCoroutine(AfterBossDeathTimer(3));
    }

    IEnumerator AfterBossDeathTimer(float value)
    {
        yield return new WaitForSeconds(value);
        OnTowerBossDead?.Invoke();
        Debug.Log("Boss has died");
    }

    IEnumerator AfterFadeUnsubscribe()
    {
        yield return new WaitForSeconds(5f);
        FadeAnimationScript.OnFaded -= (OnScreenFadeAfterDeath.Invoke);
        Debug.Log("Unsubscribing from event");
        gameObject.SetActive(false);
    }

}
