using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : Attack
{
    public GameObject projectilePrefab;
    
    public float projectileSpeed;

    public override void HandlePrimaryAttack()
    {
        Rigidbody2D projectile = Instantiate(projectilePrefab, _attackPosition.position, _attackPosition.rotation).GetComponent<Rigidbody2D>();
        projectile.AddForce(_attackPosition.right * projectileSpeed, ForceMode2D.Impulse);
        _onPrimaryAttack?.Invoke();
    }
}
