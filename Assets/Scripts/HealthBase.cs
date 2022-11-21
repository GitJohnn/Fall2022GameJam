using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class HealthBase : MonoBehaviour
{
    [SerializeField, ReadOnly] private float currentHealth;
    [SerializeField] private bool isDead = false;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    public bool IsDead => isDead;

    public abstract void ResetHealth();

    private void Awake() {
        ResetHealth();
    }

    protected virtual void InitializeHealth(float startingHealth)
    {
        currentHealth = startingHealth;
        isDead = false;
    }

    public void GetHit(int amount, GameObject sender)
    {
        if (isDead)
            return;
        if (sender.layer == gameObject.layer)
            return;

        currentHealth -= amount;

        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
            Debug.Log("hit!");
        }
        else
        {
            Die(sender);
        }
    }

    protected virtual void Die(GameObject sender) {
        OnDeathWithReference?.Invoke(sender);
        isDead = true;
        Destroy(gameObject);
    }
}
