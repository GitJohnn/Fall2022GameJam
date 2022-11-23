using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class HealthBase : MonoBehaviour
{
    [SerializeField, ReadOnly] protected float currentHealth;
    [SerializeField] private bool isDead = false;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;
    public event Action OnDeath;

    public bool IsDead => isDead;

    public abstract void ResetHealth();

    private void Start() {
        ResetHealth();
    }

    protected virtual void InitializeHealth(float startingHealth)
    {
        currentHealth = startingHealth;
        isDead = false;
    }

    public virtual void GetHit(int amount, GameObject sender)
    {
        if (isDead)
            return;
        if (sender.layer == gameObject.layer)
            return;
        
        currentHealth -= amount;

        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            Die(sender);
        }
    }

    protected virtual void Die(GameObject sender) {
        OnDeathWithReference?.Invoke(sender);
        OnDeath?.Invoke();
        isDead = true;
        Destroy(gameObject);
    }
}
