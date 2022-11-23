using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthBase {

    [SerializeField] private PlayerStats playerStats;

    public event Action<float> PlayerHit;

    public override void ResetHealth() {
        InitializeHealth(playerStats.MaxHealth);
    }

    public override void GetHit(int amount, GameObject sender) {
        base.GetHit(amount, sender);
        PlayerHit?.Invoke(currentHealth);
    }

    protected override void Die(GameObject sender) {
        base.Die(sender);
        //TODO: other stuff
    }
}