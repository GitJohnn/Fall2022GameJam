using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthBase {

    [SerializeField] private PlayerStats playerStats;
	[SerializeField] private float toxicityDamageAmount;
	[SerializeField] private float toxicityDamageTickSpeed;

	private bool toxicityFull;
	private float toxicityTickTimer;

    public event Action<float> PlayerHit;

	private void OnEnable() {
		playerStats.MaxHealthChanged += AddMaxHealth;
		playerStats.ToxicityFull += StartToxicityTickDamage;
		playerStats.StatsReset += EndToxicityTickDamage;
	}

	private void OnDisable() {
		playerStats.MaxHealthChanged -= AddMaxHealth;
		playerStats.ToxicityFull -= StartToxicityTickDamage;
		playerStats.StatsReset -= EndToxicityTickDamage;
	}

	private void Awake() {
		toxicityFull = false;
		toxicityTickTimer = 0;
	}

	private void Update() {
		toxicityTickTimer += Time.deltaTime;
		if(toxicityFull && (toxicityTickTimer >= toxicityDamageTickSpeed)) {
			GetHit(toxicityDamageAmount, FindObjectOfType<Camera>().gameObject); //just using the camera to guarantee that it's not the same layer
			toxicityTickTimer = 0;
		}
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
		if (ScreenShakeController.Instance) ScreenShakeController.Instance.StartShake(0.15f, 0.1f); 
        PlayerHit?.Invoke(currentHealth);
    }

    protected override void Die(GameObject sender) {
        base.Die(sender);
        GetComponent<PlayerDeath>().OnPlayerDeath();
        //TODO: other stuff
    }

	private void StartToxicityTickDamage() {
		toxicityFull = true;
		toxicityTickTimer = 0;
	}

	private void EndToxicityTickDamage() {
		toxicityFull = false;
	}
}