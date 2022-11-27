using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using SoundSystem;

public abstract class HealthBase : MonoBehaviour
{
    [SerializeField, ReadOnly] protected float currentHealth;
    [SerializeField] private bool isDead = false;
    [SerializeField] GameObject deathParticles;
    [SerializeField] SFXEvent _sfxHit;
    [SerializeField] SFXEvent _sfxDeath;
    [SerializeField] GameObject _vfxHit;
    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;
    public UnityEvent onDontDestroyDeath;
    public event Action OnDeath;

    public bool IsDead => isDead;
    public bool dontDestroyOnDeath = false;

    protected float currentMaxHealth;

    public abstract void ResetHealth();

    private void Start() {
        ResetHealth();
    }

    protected virtual void InitializeHealth(float startingHealth)
    {
        currentHealth = startingHealth;
        currentMaxHealth = startingHealth;
        isDead = false;
    }

    public virtual void AddHealth(float hp)
    {
        currentHealth += hp;
        if (currentHealth > currentMaxHealth) currentHealth = currentMaxHealth;
    }

    public virtual void GetHit(float amount, GameObject sender)
    {
        if (isDead)
            return;
        if (sender.layer == gameObject.layer)
            return;

        _sfxHit.Play();
        Utility.SpawnParticles(_vfxHit, gameObject, false);
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
        if (dontDestroyOnDeath)
            onDontDestroyDeath?.Invoke();
        else
            Utility.SpawnParticles(deathParticles, this.gameObject, false);
            _sfxDeath.Play();
            Destroy(gameObject);

    }
}
