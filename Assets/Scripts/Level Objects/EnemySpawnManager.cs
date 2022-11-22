using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField, HighlightIfNull] Transform _firstTransform;
    [SerializeField, HighlightIfNull] Transform _secondTransform;
    [SerializeField] Transform _playerTransform;
    public bool enableSpawn = true;
    [SerializeField] List<GameObject> _enemyObjects;
    [SerializeField] int _numOfSpawnsLimit = 10;
    [SerializeField] float _timeToSpawn = 2f;
    [SerializeField] float _timeBetweenWaves = 5f;
    [SerializeField] bool _enableGizmos = true;
    [SerializeField] UnityEvent _OnFinishedSpawns;
    [SerializeField] UnityEvent _OnAllEnemyDead;
    public event Action OnFinishedSpawns;
    public event Action OnAllEnemyDead;
    [SerializeField, ReadOnly] int _enemiesSpawned;
    [SerializeField, ReadOnly] int _enemiesKilled; 
    public int EnemiesSpawned => _enemiesSpawned;
    public int EnemiesKilled => _enemiesKilled;
    bool _runOnce;
    bool _runOnce2;
    

    void Start()
    {
        if (_playerTransform == null) _playerTransform = FindObjectOfType<PlayerMovement>().transform;
        _runOnce = false;
    }

    void Update()
    {
        if (Time.time >= _timeToSpawn && enableSpawn)
        {
            Spawn();
            _timeToSpawn = Time.time + UnityEngine.Random.Range(0.1f, _timeBetweenWaves);
        }

        if (_enemiesSpawned >= _numOfSpawnsLimit)
        {
            if (!_runOnce)
            {
                OnFinishedSpawns?.Invoke();
                _OnFinishedSpawns?.Invoke();
                _runOnce = true;
            }
            enableSpawn = false;
        }

        if (_enemiesKilled >= _numOfSpawnsLimit)
        {
            if (!_runOnce2)
            {
                OnAllEnemyDead?.Invoke();
                _OnAllEnemyDead?.Invoke();
                ResetSubscriptions();
                // Debug.Log("It WOrks");
                _runOnce2 = true;
            }
        }
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
