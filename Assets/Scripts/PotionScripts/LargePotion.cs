using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargePotion : MonoBehaviour {

    public PotionBase potion;

    public static event Action LargePotionTaken;

    private Collider2D playerTrigger;
    private bool playerInsideTrigger;

    private void Awake() {
        playerTrigger = GetComponent<Collider2D>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.E)) {
            //TODO: apply potion
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        var player = collision.GetComponent<PlayerMovement>();
        if(player) playerInsideTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        var player = collision.GetComponent<PlayerMovement>();
        if(player) playerInsideTrigger = false;
    }
}