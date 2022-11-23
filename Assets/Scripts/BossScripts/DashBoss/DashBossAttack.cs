using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashBossAttack : MonoBehaviour
{
    public int headAttackDamage = 25;
    [SerializeField] UnityEvent onDamageTarget;
    public bool showGizmos = false;
    public Transform attackPos;
    public float attackRadius = 2f;
    public float attackCooldown;
    [SerializeField] private LayerMask hitLayer;

    public bool CanAttack { get; set; } = false;

    private DashBossMovement dashBossMovement;
    private float currentAttackCooldown;

    private void Awake()
    {
        dashBossMovement = GetComponent<DashBossMovement>();
        ResetAttackCooldown();
    }

    private void OnEnable()
    {        
        dashBossMovement.onEndDash.AddListener(ResetAttackCooldown);
    }

    private void OnDisable()
    {
        dashBossMovement.onEndDash.RemoveListener(ResetAttackCooldown);
    }

    // Update is called once per frame
    void Update()
    {
        if (DashBossScript.isBossDead)
            return;
        if (!CanAttack)
            return;

        if (currentAttackCooldown < attackCooldown)
            currentAttackCooldown += Time.deltaTime; 
        else
            HandleAttackLogic();
    }

    private void HandleAttackLogic()
    {
        Debug.Log("Is Attacking");
        Collider2D[] targets = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, hitLayer);

        if (targets.Length == 0)
            return;

        foreach(Collider2D target in targets)
        {
            DamageCollider(target);            
        }
    }

    private void DamageCollider(Collider2D value)
    {
        HealthBase targetHealth = value.GetComponent<HealthBase>();
        if (!targetHealth) return;

        currentAttackCooldown = 0;
        targetHealth.GetHit(headAttackDamage, gameObject);
        onDamageTarget?.Invoke();
        Debug.Log("Hit player");
    }

    public void ResetAttackCooldown()
    {
        currentAttackCooldown = attackCooldown;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRadius);
    }



}
