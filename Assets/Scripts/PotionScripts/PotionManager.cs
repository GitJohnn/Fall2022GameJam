using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PotionManager : MonoBehaviour {

    [SerializeField] private List<PotionObject> potionObjects;
    [SerializeField] private List<PotionBase> possiblePotions;

	[SerializeField] UnityEvent PotionSelected;

    private void OnEnable() {
		foreach (var p in potionObjects) {
			p.PotionObjectTaken += OnPotionSelected;
		}
        //EnemySpawnManager.OnAllEnemyDead += ActivatePotions;
    }

    private void OnDisable() {
		foreach(var p in potionObjects) {
			p.PotionObjectTaken -= OnPotionSelected;
		}
		//EnemySpawnManager.OnAllEnemyDead -= ActivatePotions;
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
    public void ActivatePotions() {
        foreach(var o in potionObjects) {
            o.gameObject.SetActive(true);
        }
    }

	private void OnPotionSelected() {
		PotionSelected?.Invoke();
		DeactivatePotions();
	}

    //deactivate all potions in the scene once on is taken
    private void DeactivatePotions() {
        foreach(var o in potionObjects) {
            o.gameObject.SetActive(false);
        }
    }
}