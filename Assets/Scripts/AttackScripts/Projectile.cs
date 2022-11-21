using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int projectileDamage = 5;
    public LayerMask collisionLayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var targetHealth = other.GetComponent<Health>();
        if (!targetHealth) return;

        targetHealth.GetHit(projectileDamage, gameObject);
    }
}
