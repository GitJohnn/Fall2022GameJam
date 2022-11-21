using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthBase {

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider healthBar;

    private void OnEnable() {
        playerStats.MaxHealthChanged += UpdateHealthBar;
    }

    private void OnDisable() {
        playerStats.MaxHealthChanged -= UpdateHealthBar;
    }

    public override void ResetHealth() {
        playerStats.InitializeStats(); //TODO: move this somewhere else!
        InitializeHealth(playerStats.MaxHealth);
    }

    private void UpdateHealthBar(float newMaxHealth) {
        //TODO: make ui changes to reflect health change
    }

    protected override void Die(GameObject sender) {
        base.Die(sender);
        //TODO: other stuff
    }
}