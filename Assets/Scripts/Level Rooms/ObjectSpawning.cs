using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectSpawning : MonoBehaviour
{
    [SerializeField] private RoomData _roomData = null;

    // Start is called before the first frame update
    void Start()
    {
        if (_roomData)
        {
            _roomData.SpawnObjects(this.transform);
            _roomData.SpawnDoors();
        }
    }
}
