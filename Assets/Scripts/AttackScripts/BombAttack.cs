using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAttack : Attack
{
    public ButtonCoolDownHandler attackButton;
    public GameObject bombPrefab;

    public bool bombAttackUnlocked = true;
    public float totalCooldown;

    public void Awake()
    {
        if (bombAttackUnlocked)
            BombUnlocked();
        else
            BombLocked();
    }

    private void Update()
    {
        attackButton.CurrentAbilityCooldown = currentAttackCooldown;
    }

    public void BombUnlocked()
    {
        canAttack = true;
        attackButton.SetTotalcooldown(totalCooldown);
        currentAttackCooldown = totalCooldown;
    }

    private void BombLocked()
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

        GameObject bombrb = Instantiate(bombPrefab, _attackPosition.position, Quaternion.identity);
        //projectile.AddForce(_attackPosition.right * projectileSpeed, ForceMode2D.Impulse);

        while (currentAttackCooldown <= totalCooldown)
        {
            currentAttackCooldown += Time.deltaTime;
            yield return null;
        }

        canAttack = true;
    }
}
