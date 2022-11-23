using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "CreateRoomSpawnData")]
public class RoomData: ScriptableObject
{
    [SerializeField] private Vector2 _roomDimentions = new Vector2();
    [SerializeField] private List<ObjectSpawnData> _objectData = new List<ObjectSpawnData>();
    [SerializeField] private List<Transform> _doorLocations = new List<Transform>();

    public Vector2 RoomDimentions => _roomDimentions;
    public List<ObjectSpawnData> ObjectSpawnData => _objectData;
    public List<Transform> DoorLocations => _doorLocations;

    public void SpawnRoomObjects()
    {
        foreach (var objectData in _objectData)
        {
            if (objectData) objectData.SpawnObject();
        }
    }
}
