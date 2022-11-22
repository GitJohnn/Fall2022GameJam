using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionObject : MonoBehaviour {

    public PotionBase potion;

    public static event Action PotionObjectTaken;

    private PlayerStats player;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.E) && player != null) {
            potion.ApplyPotion(player);
            PotionObjectTaken?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        player = collision.GetComponent<PlayerStats>();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        var thisPlayer = collision.GetComponent<PlayerStats>();
        if(thisPlayer) player = null;
    }

    public void AssignPotion(PotionBase newPotion) {
        potion = newPotion;
        //TODO: assign the UI stuff here too
    }
}