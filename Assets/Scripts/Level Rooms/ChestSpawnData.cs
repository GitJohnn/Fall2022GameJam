using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu (menuName = "ChestSpawnData")]
public class ChestSpawnData : ScriptableObject
{
    [SerializeField] private GameObject _chestPrefab = null;
    [SerializeField] private int _numOfChest = 1;
    [SerializeField] private List<Vector2> _spawnedLocations = new List<Vector2>();
    [Range(0, 1)]
    [SerializeField] private float _chanceToSpawnChest = 1;

    public void SpawnObject(List<Vector2> spawns)
    {
        if (_spawnedLocations.Count != 0) _spawnedLocations.RemoveRange(0,_spawnedLocations.Count);
        for (int i = 0; i < _numOfChest; i++)
        {
            var randomInt = UnityEngine.Random.Range(0, spawns.Count);
            if (UnityEngine.Random.Range(0f, 1f) <= _chanceToSpawnChest)
            {
                var currentSpawnLocation = spawns[randomInt];
                var newObject = Instantiate(_chestPrefab);
                newObject.transform.position = Vector2.zero;
                newObject.transform.position = currentSpawnLocation;
                _spawnedLocations.Add(currentSpawnLocation);
                spawns.Remove(currentSpawnLocation);
            }
        }
    }


}
