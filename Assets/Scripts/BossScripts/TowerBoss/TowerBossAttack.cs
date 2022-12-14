using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class TowerBossAttack : MonoBehaviour
{
    public Rigidbody2D projectilePrefab;
    public Transform shootingPosition;
    public float projectileSpeed = 8;
    public float phase1ShootingCoolDown = 1.25f;
    public float phase2ShootingCoolDown = 1.25f;
    public bool StopAttacking { get; set; } = true;

    public Transform playerTransform; 
    private float currentShootingCooldown = 0;
    private bool isPhase2 = false;

    [SerializeField] SFXEvent _spiderShot;

    private void Awake()
    {
        if (!playerTransform)
            playerTransform = GameObject.FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (StopAttacking || TowerBossScript.isBossDead)
            return;

        float totalCooldown = isPhase2 ? phase2ShootingCoolDown : phase1ShootingCoolDown;

        if(currentShootingCooldown < totalCooldown)
        {
            currentShootingCooldown += Time.deltaTime;
            return;
        }
        else
        {
            currentShootingCooldown = 0;
            if (isPhase2)
                ShootPlayer();
            else
                ShootStraight();
        }
    }

    private void ShootStraight()
    {
        _spiderShot.Play();
        Rigidbody2D projectile = Instantiate(projectilePrefab, shootingPosition.position, shootingPosition.rotation).GetComponent<Rigidbody2D>();
        projectile.AddForce(shootingPosition.right * projectileSpeed, ForceMode2D.Impulse);
    }

    private void ShootPlayer()
    {
        _spiderShot.Play();
        Vector2 playerDir = (playerTransform.position - shootingPosition.position).normalized;
        Rigidbody2D projectile = Instantiate(projectilePrefab, shootingPosition.position, shootingPosition.rotation).GetComponent<Rigidbody2D>();
        projectile.transform.right = playerDir;
        projectile.AddForce(playerDir * projectileSpeed, ForceMode2D.Impulse);
    }

    public void StartPhase2()
    {
        //playerTransform = value;
        isPhase2 = true;
    }

}
