using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionObject : MonoBehaviour {

	[SerializeField] private PotionInfoPanel potionInfoPanel;
    private PotionBase potion;
	private PlayerStats player;

	public static event Action PotionObjectTaken;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.F) && player != null) {
            potion.ApplyPotion(player);
            PotionObjectTaken?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        player = collision.GetComponent<PlayerStats>();
		if(player) {
			potionInfoPanel.ShowPanel();
		}
    }

    private void OnTriggerExit2D(Collider2D collision) {
        var thisPlayer = collision.GetComponent<PlayerStats>();
		if(thisPlayer) {
			player = null;
			potionInfoPanel.HidePanel();
		}
    }

	private void OnMouseOver() {
		potionInfoPanel.ShowPanel();
	}

	private void OnMouseExit() {
		potionInfoPanel.HidePanel();
	}

	public void AssignPotion(PotionBase newPotion) {
        potion = newPotion;
		potionInfoPanel.InitializePanel(potion);
    }
}