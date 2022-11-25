using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RangedAttack : Attack
{
    public ButtonCoolDownHandler attackButton;
    public GameObject projectilePrefab;

    public bool rangeAttackUnlocked = true;
    public float projectileSpeed;
    public float totalCooldown;


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

    public float currentAttackCooldown { get; set; }

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

        Rigidbody2D projectile = Instantiate(projectilePrefab, _attackPosition.position, _attackPosition.rotation).GetComponent<Rigidbody2D>();
        projectile.AddForce(_attackPosition.right * projectileSpeed, ForceMode2D.Impulse);

        while(currentAttackCooldown <= totalCooldown)
        {
            currentAttackCooldown += Time.deltaTime;
            yield return null;
        }

        canAttack = true;
    }

}
