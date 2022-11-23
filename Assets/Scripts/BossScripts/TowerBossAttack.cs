using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBossAttack : MonoBehaviour
{
    public Rigidbody2D projectilePrefab;
    public Transform shootingPosition;
    public float projectileSpeed = 8;
    public float shootingCoolDown = 1.25f;

    public Transform playerTransform; 
    private float currentShootingCooldown = 0;

    private void Awake()
    {
        //SetPlayerTransform(player);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentShootingCooldown < shootingCoolDown)
        {
            currentShootingCooldown += Time.deltaTime;
            return;
        }
        else
        {
            currentShootingCooldown = 0;
            ShootPlayer();
        }
    }

    private void ShootStraight()
    {
        Rigidbody2D projectile = Instantiate(projectilePrefab, shootingPosition.position, shootingPosition.rotation).GetComponent<Rigidbody2D>();
        projectile.AddForce(shootingPosition.right * projectileSpeed, ForceMode2D.Impulse);
    }

    private void ShootPlayer()
    {
        Vector2 playerDir = (playerTransform.position - shootingPosition.position).normalized;
        Rigidbody2D projectile = Instantiate(projectilePrefab, shootingPosition.position, shootingPosition.rotation).GetComponent<Rigidbody2D>();
        projectile.transform.right = playerDir;
        projectile.AddForce(playerDir * projectileSpeed, ForceMode2D.Impulse);
    }

    public void SetPlayerTransform(Transform value)
    {
        playerTransform = value;
    }

}
