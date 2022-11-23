using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : MonoBehaviour {

    [SerializeField] private List<PotionObject> potionObjects;
    [SerializeField] private List<PotionBase> possiblePotions;

    private void OnEnable() {
        PotionObject.PotionObjectTaken += DeactivatePotions;
        EnemySpawnManager.OnAllEnemyDead += ActivatePotions;
    }

    private void OnDisable() {
        PotionObject.PotionObjectTaken -= DeactivatePotions;
        EnemySpawnManager.OnAllEnemyDead -= ActivatePotions;
    }

    private void Start() {
        //randomly assign each potion object to a random possible potion
        var unusedPotions = possiblePotions;
        foreach(var o in potionObjects) {
            int randInd = Random.Range(0, unusedPotions.Count);
            o.AssignPotion(unusedPotions[randInd]);
            unusedPotions.RemoveAt(randInd);
        }
        //deactivate the potions
        DeactivatePotions();
    }

    //activate all potions in the scene
    private void ActivatePotions() {
        foreach(var o in potionObjects) {
            o.gameObject.SetActive(true);
        }
    }

    //deactivate all potions in the scene once on is taken
    private void DeactivatePotions() {
        foreach(var o in potionObjects) {
            o.gameObject.SetActive(false);
        }
    }
}