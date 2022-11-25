using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField, HighlightIfNull] Transform _firstTransform;
    [SerializeField, HighlightIfNull] Transform _secondTransform;
    [SerializeField] Transform _playerTransform;
    public bool enableSpawn = false;
    [Header("Settings")]
    [SerializeField] List<GameObject> _enemyObjects;
    [SerializeField] int _numOfSpawnsLimit = 10;
    [SerializeField] float _timeToSpawn = 2f;
    [SerializeField] float _timeBetweenWaves = 5f;
    [SerializeField] bool _enableGizmos = true;
    [Header("Events")]
    [SerializeField] UnityEvent _OnFirstSpawn;
    [SerializeField] UnityEvent _OnFinishedSpawns;
    [SerializeField] UnityEvent _OnAllEnemyDead;
    public static event Action OnFirstSpawn;
    public static event Action OnFinishedSpawns;
    public static event Action OnAllEnemyDead;
    [Header("Debug")]
    [SerializeField, ReadOnly] int _enemiesSpawned;
    [SerializeField, ReadOnly] int _enemiesKilled; 
    public int EnemiesSpawned => _enemiesSpawned;
    public int EnemiesKilled => _enemiesKilled;
    bool _runOnce = false;
    bool _runOnce2 = false;

    

    void Start()
    {
        if (_playerTransform == null) _playerTransform = FindObjectOfType<PlayerMovement>().transform;
        _runOnce = false;
    }

    [Button]
    public void StartSpawning()
    {
        HandleFirstSpawn();
        enableSpawn = true;
    }

    void Update()
    {
        if (!enableSpawn) return;

        if (Time.time >= _timeToSpawn)
        {
            Spawn();
            _timeToSpawn = Time.time + UnityEngine.Random.Range(0.1f, _timeBetweenWaves);

        }

        if (_enemiesSpawned >= _numOfSpawnsLimit)
        {
            if (!_runOnce) HandleFinishedSpawns();
            enableSpawn = false;
        }

        if (_enemiesKilled >= _numOfSpawnsLimit)
        {
            if (!_runOnce2) HandleEnemiesKilled();
        }
    }

    void HandleFirstSpawn()
    {
        OnFirstSpawn?.Invoke();
        _OnFirstSpawn?.Invoke();
    }

    void HandleFinishedSpawns()
    {
        OnFinishedSpawns?.Invoke();
        _OnFinishedSpawns?.Invoke();
        _runOnce = true;
    }

    void HandleEnemiesKilled()
    {
        OnAllEnemyDead?.Invoke();
        _OnAllEnemyDead?.Invoke();
        ResetSubscriptions();
        // Debug.Log("It WOrks");
        _runOnce2 = true;
    }

    void ResetSubscriptions()
    {
        for (int i = 0; i < _enemyObjects.Count; i++)
        {
            HealthBase enemyHealth = _enemyObjects[i].GetComponent<HealthBase>();
            if (enemyHealth != null)
            {
                enemyHealth.OnDeath -= OnEnemyDeath;
            } 
        }
    }

    void OnEnemyDeath()
    {
        _enemiesKilled++;
    }

    void Spawn()
    {
        GameObject enemyToSpawn = _enemyObjects[UnityEngine.Random.Range(0, _enemyObjects.Count)];
        float randomXPosition = UnityEngine.Random.Range(_firstTransform.position.x, _secondTransform.position.x);
        float randomYPosition = UnityEngine.Random.Range(_firstTransform.position.y, _secondTransform.position.y);
        Vector2 spawnPosition = new Vector2(randomXPosition, randomYPosition);

        GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        AIData enemyData = spawnedEnemy.GetComponent<AIData>();
        HealthBase enemyHealth = spawnedEnemy.GetComponent<HealthBase>();
        enemyHealth.OnDeath += OnEnemyDeath;
        enemyData.currentTarget = _playerTransform;

        _enemiesSpawned++;
    }

    void OnDrawGizmos()
    {
        if (_enableGizmos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_firstTransform.position, 1f);
            Gizmos.DrawWireSphere(_secondTransform.position, 1f);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
    }

}
