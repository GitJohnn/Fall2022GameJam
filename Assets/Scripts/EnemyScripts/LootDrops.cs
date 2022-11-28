using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class LootDrops : MonoBehaviour
{
    [SerializeField] List<GameObject> _loot;
    [SerializeField] float _dropChance;
    [SerializeField, HighlightIfNull] HealthBase _enemyHealth;

    void OnValidate()
    {
        if (_enemyHealth == null) _enemyHealth = GetComponent<HealthBase>();
    }

    [Button]
    public void Drop()
    {
        if (_loot.Count <= 0) return;
        float randNum = Random.Range(0, 100);
        if (randNum <= _dropChance)
        {
            GameObject dropped = Instantiate(_loot[Random.Range(0, _loot.Count)], transform.position, Quaternion.identity);
	        //Debug.Log($"Created: {dropped}");
        }
    }

   
}

