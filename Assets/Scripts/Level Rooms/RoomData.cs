using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "CreateRoomSpawnData")]
public class RoomData: ScriptableObject
{
    [SerializeField] public Vector2 _roomDimentions = new Vector2();
    [SerializeField] public List<ObjectSpawnData> _objectData = new List<ObjectSpawnData>();
    [SerializeField] public List<Vector2> _doorLocations = new List<Vector2>();


    public void SpawnRoomObjects()
    {
        foreach (var objectData in _objectData)
        {
            objectData.SpawnObject();
        }
    }
}
