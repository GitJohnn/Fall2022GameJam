using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "CreateRoomSpawnData")]
public class SpawnRoomData: ScriptableObject
{
    [SerializeField] private ChestSpawnData _chestSpawnData;

    public void SpawnRoomObjects(List<Vector2> spawns)
    {
        _chestSpawnData.SpawnObject(spawns);
    }
}
