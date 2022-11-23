using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ObjectSpawnData")]
public class ObjectSpawnData : ScriptableObject
{
    [Header("Spawn Data")]
    [SerializeField] protected GameObject _prefabToSpawn = null;

    [SerializeField] protected int _maxNumToSpawn = 1;
    [SerializeField] protected int _guaranteedNumToSpawn = 0;
    [SerializeField] public List<Vector2> _locationsToSpawn = new List<Vector2>();

    [Range(0, 1)]
    [SerializeField] protected float _chanceToSpawnObject = 0.5f;

    private List<Vector2> _spawnedLocations = new List<Vector2>();
    protected int _numCurrentlySpawned = 0;
    private bool _validSpawn = false;
    private Vector2 _currenSpawnLocation;

    public void SpawnObject()
    {
        _numCurrentlySpawned = 0;
        if (_spawnedLocations.Count != 0) _spawnedLocations.RemoveRange(0, _spawnedLocations.Count);
        for (int i = 0; i < _maxNumToSpawn; i++)
        {
            if (UnityEngine.Random.Range(0f, 1f) <= _chanceToSpawnObject)
            {
                while (!_validSpawn)
                {
                    _currenSpawnLocation = _locationsToSpawn[UnityEngine.Random.Range(0, _locationsToSpawn.Count)];
                    if (_spawnedLocations.Contains(_currenSpawnLocation)) _validSpawn = false;
                    else
                    {
                        _spawnedLocations.Add(_currenSpawnLocation);
                        _validSpawn = true;
                        break;
                    }
                }

                var newObject = Instantiate(_prefabToSpawn);
                newObject.transform.position = Vector2.zero;
                newObject.transform.position = _currenSpawnLocation;
                _numCurrentlySpawned++;
                _validSpawn = false;
                Debug.Log("Random Spawn");
            }
        }

        if (_numCurrentlySpawned < _guaranteedNumToSpawn)
        {
            for (int i = _numCurrentlySpawned; i < _guaranteedNumToSpawn; i++)
            {
                while (!_validSpawn) 
                {
                    _currenSpawnLocation = _locationsToSpawn[UnityEngine.Random.Range(0, _locationsToSpawn.Count)];
                    if (_spawnedLocations.Contains(_currenSpawnLocation)) _validSpawn = false;
                    else
                    {
                        _spawnedLocations.Add(_currenSpawnLocation);
                        _validSpawn = true;
                        break;
                    }
                }

                var newObject = Instantiate(_prefabToSpawn);
                newObject.transform.position = Vector2.zero;
                newObject.transform.position = _currenSpawnLocation;
                _validSpawn = false;
                Debug.Log("Guaranteed Spawn");
            }
        }
    }
}
