using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RangedAttack : Attack
{
    public float CurrentAttackCooldown { get; set; }    
    public GameObject projectilePrefab;

    public float projectileSpeed;
    public float totalDashcooldown;


    public void Awake()
    {
        //enemyAI = GetComponent<EnemyAI>();
    }

    public override void HandlePrimaryAttack()
    {
        Rigidbody2D projectile = Instantiate(projectilePrefab, _attackPosition.position, _attackPosition.rotation).GetComponent<Rigidbody2D>();
        projectile.AddForce(_attackPosition.right * projectileSpeed, ForceMode2D.Impulse);
        _onPrimaryAttack?.Invoke();
    }

}
