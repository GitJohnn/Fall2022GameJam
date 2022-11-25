using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class LootDrops : MonoBehaviour
{
    [SerializeField] List<LootItem> _loot;
    [SerializeField, HighlightIfNull] HealthBase _enemyHealth;

    void OnValidate()
    {
        if (_enemyHealth == null) _enemyHealth = GetComponent<HealthBase>();
    }

    public void Drop()
    {
        GameObject objectToSpawn = HandleObjectPick();
        Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }

    GameObject HandleObjectPick()
    {
        if (_loot.Count <= 0) return null;

        int randomPercentage = Random.Range(0, 101);
        List<GameObject> possibleObjects = new List<GameObject>();

        foreach (var item in _loot)
        {
            if (item.dropPercentage <= randomPercentage) possibleObjects.Add(item.lootDrop);
        }

        return possibleObjects[Random.Range(0, possibleObjects.Count)];
    }
}

[Serializable]
public struct LootItem
{
    public GameObject lootDrop;
    public int dropPercentage;
}
