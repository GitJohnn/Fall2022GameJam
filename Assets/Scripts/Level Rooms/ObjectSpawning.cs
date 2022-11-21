using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectSpawning : MonoBehaviour
{
    [SerializeField] private SpawnRoomData _spawnRoomData = null;
    [SerializeField] public List<Vector2> _chestSpawnLocations = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        _spawnRoomData.SpawnRoomObjects(_chestSpawnLocations);
    }

    private void OnDrawGizmosSelected()
    {
        if (_spawnRoomData)
        {
            foreach (var location in _chestSpawnLocations)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(location, .5f);
            }
        }

    }
}
