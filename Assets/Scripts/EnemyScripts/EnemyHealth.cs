using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthBase {

    [SerializeField] private float maxHealth;

    public override void ResetHealth() {
        InitializeHealth(maxHealth);
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}