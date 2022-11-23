using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnObjectsInRoom : MonoBehaviour
{
    [SerializeField] private RoomData _roomData = null;

    // Start is called before the first frame update
    void Start()
    {
        
        if (_roomData) _roomData.SpawnRoomObjects();
    }

    private void OnDrawGizmosSelected()
    {
        if (_roomData)
        {
            foreach (var spawnData in _roomData._objectData)
            {
                foreach (var location in spawnData._locationsToSpawn)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(location, .5f);
                }

            }

            foreach (var doorLocation in _roomData._doorLocations)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(doorLocation,Vector3.one);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, _roomData._roomDimentions);
        }

    }
}
