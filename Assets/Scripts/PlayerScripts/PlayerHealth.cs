using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthBase {

    [SerializeField] private PlayerStats playerStats;
	[SerializeField] private float toxicityDamageAmount;
	[SerializeField] private float toxicityDamageTickSpeed;
    [SerializeField] private ParticleSystem _toxicityDamagePS = null;

	private bool toxicityFull;
	private float toxicityTickTimer;

    public event Action<float> PlayerHit;
	public event Action<float> PlayerHealed;

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
            _toxicityDamagePS.Play();
			toxicityTickTimer = 0;
		}
	}

	public override void ResetHealth() {
        InitializeHealth(playerStats.MaxHealth);
    }

	public void HealToFull() {
		AddHealth(playerStats.MaxHealth - currentHealth);
	}

	public override void AddHealth(float hp) {
		base.AddHealth(hp);
		PlayerHealed?.Invoke(currentHealth);
	}

	private void AddMaxHealth(float newMaxHealth) {
		float healthDifference = newMaxHealth - currentMaxHealth;
		currentMaxHealth = newMaxHealth;
		if(healthDifference > 0) {
			AddHealth(healthDifference);
		} else if(currentHealth > newMaxHealth) {
			float healthOverflow = newMaxHealth - currentHealth;
			AddHealth(healthOverflow); //takes away health so that the current health is equal to the max
		}
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