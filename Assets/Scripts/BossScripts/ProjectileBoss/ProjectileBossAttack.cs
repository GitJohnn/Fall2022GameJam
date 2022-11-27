using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SoundSystem;
public class ProjectileBossAttack : MonoBehaviour
{
    public Rigidbody2D projectilePrefab;
    public Transform[] Turret1Pos;
    public Transform[] Turret2Pos;
    public UnityEvent OnTurretsDestroyed;
    public UnityEvent OnStartPhase2;
    public Transform playerPos;
    public float projectileSpeed = 8;
    public float stopMovementCooldown = 5f;
    public float shootingCooldown;
    public float phase2ShootCooldown = 2f;

    public bool StopAttacking { get; set; } = true;

    private bool IsPhase2 = false;
    private int turretIndex = 0;
    private float currentTurretCooldown = 0;
    private int turret1DestroyCount = 0;
    private int turret2DestroyCount = 0;
    private bool turret1Destroyed = false;
    private bool turret2Destroyed = false;

    [SerializeField] SFXEvent _robotShot;

    // Update is called once per frame
    void Update()
    {
        if (ProjectileBossScipt.IsBossDead)
            return;

        if (StopAttacking)
            return;

        if (currentTurretCooldown < shootingCooldown)
        {
            currentTurretCooldown += Time.deltaTime;
            return;
        }
        else
        {
            currentTurretCooldown = 0;
            ShootTurret();
        }
    }

    private void ShootTurret()
    {
        turretIndex = (turretIndex < 1) ? (turretIndex + 1) : 0;
        Transform[] turretPosArray = turretIndex == 0 ? Turret1Pos : Turret2Pos;

        if (!IsPhase2)
            ShootStraight(turretPosArray);
        else if(!isPhase2Shooting)
        {
            isPhase2Shooting = true;
            StartCoroutine(Phase2ShootingCoroutine());
            //ShootToPlayer(turretPosArray);
        }
        //ShootRandomStraight();
        //else
        //ShootToPlayer();
    }

    private void ShootStraight(Transform[] turretPositions)
    {
        //float randomRotation = Random.Range(-30, 30);
        //Quaternion.Euler(0, 0, randomRotation) *
        if ((turret1Destroyed && turretIndex == 0)|| (turret2Destroyed && turretIndex == 1))
            return;
        _robotShot.Play();
        foreach (Transform turret in turretPositions)
        {
            Rigidbody2D projectile = Instantiate(projectilePrefab, turret.position, turret.rotation).GetComponent<Rigidbody2D>();
            projectile.AddForce(turret.right * projectileSpeed, ForceMode2D.Impulse);
        }
    }

    private void ShootToPlayer(Transform[] turretPositions)
    {
        foreach (Transform turret in turretPositions)
        {
            Vector2 playerDir = (playerPos.position - turret.position).normalized;
            Rigidbody2D projectile = Instantiate(projectilePrefab, turret.position, turret.rotation).GetComponent<Rigidbody2D>();
            projectile.transform.right = playerDir;
            projectile.AddForce(playerDir * projectileSpeed, ForceMode2D.Impulse);
        }

    }

    public void Turret1Death()
    {
        turret1Destroyed = true;
        CheckStartPhase2();
    }

    public void Turret2Death()
    {
        turret2Destroyed = true;
        CheckStartPhase2();
    }

    public void CheckStartPhase2()
    {
        if (turret1Destroyed && turret2Destroyed)
        {
            OnTurretsDestroyed?.Invoke();
            if (!IsPhase2)
            {
                StartCoroutine(StartPhase2Coroutine());
            }
        }
        
    }

    private bool isPhase2Shooting = false;

    private IEnumerator Phase2ShootingCoroutine()
    {
        float currentStraightShooting = 0;        
        while (currentStraightShooting < phase2ShootCooldown)
        {
            currentStraightShooting += Time.deltaTime;
            yield return null;
        }
        if(!turret1Destroyed)
            ShootStraight(Turret1Pos);
        if(!turret2Destroyed)
            ShootStraight(Turret2Pos);
        StartCoroutine(Phase2ShootingCoroutine());
    }

    private IEnumerator StartPhase2Coroutine()
    {
        StopAttacking = true;
        float currentPhase2 = 0;
        while(currentPhase2 < stopMovementCooldown)
        {
            currentPhase2 += Time.deltaTime;
            yield return null;
        }

        IsPhase2 = true;
        StopAttacking = false;
        OnStartPhase2?.Invoke();
        turret1Destroyed = false;
        turret2Destroyed = false;
    }
}
