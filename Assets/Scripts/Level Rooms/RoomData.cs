using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu (menuName = "RoomData")]
public class RoomData : ScriptableObject
{
    [Header("Object Spawning")]
    [SerializeField] private List<GameObject> _objectsToSpawn = new List<GameObject>();
    [SerializeField] private List<int> _numOfObject = new List<int>();
    [SerializeField] private List<Transform> _objectSpawnLocations = new List<Transform>();
    [Range(0,1)]
    [SerializeField] private List<float> _chanceToSpawnObject = new List<float>();

    [Header("Door Spawning")]
    [SerializeField] private List<GameObject> _doorsToSpawn = new List<GameObject>();
    [SerializeField] private List<Transform> _doorSpawnLocations = new List<Transform>();
    [Range(0, 1)]
    [SerializeField] private List<float> _chanceToSpawnDoor = new List<float>();
    [Range(1,4)]
    [SerializeField] private float _requiredNumDoors = 1;

    public void SpawnObjects(Transform parent)
    {
        var tempSpawnLocations = _objectSpawnLocations;
        foreach (var spawnedObject in _objectsToSpawn)
        {
            for (int i = 0; i < _numOfObject.Count; i++)
            {
                var randomInt = UnityEngine.Random.Range(0, _objectSpawnLocations.Count - 1);
                var currentSpawnLocation = _objectSpawnLocations[randomInt];
                var newObject = Instantiate(spawnedObject, parent);
                newObject.transform.position = parent.position;

            }
        }
    }

    public void SpawnDoors()
    {

    }
}
