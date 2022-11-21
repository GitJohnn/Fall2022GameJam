using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack
{
    EnemyAI enemyAI;
    GameObject projectilePrefab;
    Transform firePosition;

    public float projectileSpeed;

    public void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
    }

    public override void HandlePrimaryAttack()
    {
        Rigidbody2D projectile = Instantiate(projectilePrefab, firePosition.position, firePosition.rotation).GetComponent<Rigidbody2D>();
        projectile.AddForce(firePosition.up * projectileSpeed, ForceMode2D.Impulse);
    }

}
