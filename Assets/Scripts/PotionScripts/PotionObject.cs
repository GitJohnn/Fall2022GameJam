using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class PotionObject : MonoBehaviour {

	[SerializeField] private PotionInfoPanel potionInfoPanel;
    private PotionBase potion;
	private PlayerStats player;

	public event Action PotionObjectTaken;
    #region setup Particles
    [SerializeField] GameObject _waitingVFX;
    private void Start()
    {
        Utility.SpawnParticles(_waitingVFX, this.gameObject, true);
    }
    #endregion

    [SerializeField] SFXEvent _sfxDrinkPotion;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.I) && player != null) {
            _sfxDrinkPotion.Play();
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
		if(player) return; //player is inside trigger, don't hide it
		potionInfoPanel.HidePanel();
	}

	public void AssignPotion(PotionBase newPotion) {
        potion = newPotion;
		potionInfoPanel.InitializePanel(potion);
    }
}