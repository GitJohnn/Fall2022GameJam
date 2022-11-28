using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SoundSystem;

public class RangedAttack : Attack
{
    public ButtonCoolDownHandler attackButton;
    public GameObject projectilePrefab;
    public bool usePlayerStats = false;
    [SerializeField, ShowIf("usePlayerStats")] PlayerStats stats;
    public bool rangeAttackUnlocked = true;
    public float projectileSpeed;
    public float totalCooldown;

    [SerializeField] SFXEvent _sfxBowShoot;

    public void Awake()
    {
        if (rangeAttackUnlocked)
            RangeUnlocked();
        else
            RangeLocked();
    }

    private void Update()
    {
        attackButton.CurrentAbilityCooldown = currentAttackCooldown;
    }

    public void RangeUnlocked()
    {
        canAttack = true;
        attackButton?.SetTotalcooldown(totalCooldown);
        currentAttackCooldown = totalCooldown;
    }

    private void RangeLocked()
    {
        canAttack = false;
    }

	public float currentAttackCooldown; /*{ get; set; }*/

    bool canAttack;

    public override void HandlePrimaryAttack()
    {
        if (!canAttack)
            return;
        StartCoroutine(RangeAttack());        
        _onPrimaryAttack?.Invoke();
    }

    IEnumerator RangeAttack()
    {
        canAttack = false;
        currentAttackCooldown = 0;
        _sfxBowShoot.Play();

        Rigidbody2D projectile = Instantiate(projectilePrefab, _attackPosition.position, _attackPosition.rotation).GetComponent<Rigidbody2D>();
        projectile.AddForce(_attackPosition.right * projectileSpeed, ForceMode2D.Impulse);
        if (projectile.gameObject.GetComponent<Projectile>() && usePlayerStats) projectile.gameObject.GetComponent<Projectile>().projectileDamage = Mathf.RoundToInt(stats.AttackPower);

        while(currentAttackCooldown <= totalCooldown)
        {
            currentAttackCooldown += Time.deltaTime;
            yield return null;
        }

        canAttack = true;
    }

}
