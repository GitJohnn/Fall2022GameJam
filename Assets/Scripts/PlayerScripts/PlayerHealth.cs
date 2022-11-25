using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthBase {

    [SerializeField] private PlayerStats playerStats;

    public event Action<float> PlayerHit;

	private void OnEnable() {
		playerStats.MaxHealthChanged += AddMaxHealth;
	}

	private void OnDisable() {
		playerStats.MaxHealthChanged -= AddMaxHealth;
	}

	public override void ResetHealth() {
        InitializeHealth(playerStats.MaxHealth);
    }

	private void AddMaxHealth(float newMaxHealth) {
		float healthDifference = newMaxHealth - currentMaxHealth;
		currentMaxHealth = newMaxHealth;
		AddHealth(healthDifference); //can also take away health if the difference is negative
	}

    public override void GetHit(float amount, GameObject sender) {
		float actualAmount = amount / (playerStats.Defense / 100);
        base.GetHit(actualAmount, sender);
        PlayerHit?.Invoke(currentHealth);
    }

    protected override void Die(GameObject sender) {
        base.Die(sender);
        //TODO: other stuff
    }
}