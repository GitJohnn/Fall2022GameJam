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

    [SerializeField]public UnityEvent OnTowerBossDead;

    private bool isBossDead = false;

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
    private int phase2HealthAmount;
    private int currentPhase2Amount = 0;
    [SerializeField] UnityEvent OnStartPhase2;
    [SerializeField] UnityEvent OnPhase2Complete;

    [SerializeField] UnityEvent onStartMovePlayer;
    [SerializeField] UnityEvent onEndMovePlayer;
    private bool isMovingPlayer = false;
    private float timeToMovePlayer = 2;
    private float currentTimeToMovePlayer = 0;

    private void Awake()
    {
        phase1HealthAmount = phase1Health.Count;        
        phase2HealthAmount = phase2Health.Count;
        //Debug.Log(phase1HealthAmount + " " + phase2HealthAmount);
    }

    private void Update()
    {
        if(!isBossDead)
            towerBossHealthSlider.value = towerBossHealth.GetHealthPercentage();

        if (!isMovingPlayer)
            return;

        if(currentTimeToMovePlayer < timeToMovePlayer)
        {
            currentTimeToMovePlayer += Time.deltaTime;
            MovePlayerBeforeNextPhaseStart();
        }
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

    public void CheckPhase2HealthObstacles()
    {
        currentPhase2Amount++;

        if (currentPhase2Amount < phase2HealthAmount)
            return;

        phase2Complete = true;
        OnPhase2Complete?.Invoke();
    }

    public void MovePlayerBeforeNextPhaseStart()
    {
        Debug.Log("moving player");
        playerTransform.position = Vector3.Lerp(playerTransform.position, movePlayerPos.position, currentTimeToMovePlayer / timeToMovePlayer);
    }

    IEnumerator TimeBeforeNextPhase(float value)
    {
        yield return new WaitForSeconds(value);
        isMovingPlayer = true;
        onStartMovePlayer?.Invoke();
        yield return new WaitForSeconds(timeToMovePlayer);
        isMovingPlayer = false;
        onEndMovePlayer?.Invoke();
        OnStartPhase2?.Invoke();
    }

    public void TriggerBossDead()
    {
        isBossDead = true;
        OnTowerBossDead?.Invoke();
    }

}
