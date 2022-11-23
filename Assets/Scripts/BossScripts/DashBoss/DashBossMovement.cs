using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashBossMovement : MonoBehaviour
{
    public Transform targetTransform;
    public float totalMovementCooldown = 1.25f;
    public float totalRotateCooldown = 2f;

    [SerializeField] public UnityEvent onStartDash;
    [SerializeField] public UnityEvent onEndDash;

    public float dashDuration = 1.25f;
    public float dashSpeed = 8f;
    public float totalDashCooldown = 1.5f;    
    public float rotationSpeed = 15f;

    public bool IsStopped { get; set; } = false;

    private Rigidbody2D bossRb;
    private float currentMovementCooldown = 0;
    private float currentRotateCooldown = 0;
    private float currentDashCooldown = 0;
    private bool canRotate = false;
    private bool canDash = false;
    private bool isDashing = false;
    private bool reachedRotatePos = false;
    private bool reachedMovePos = false;

    private void Awake()
    {
        bossRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (DashBossScript.isBossDead)
            return;
        if (IsStopped)
            return;

        if (!canRotate && !canDash && !isDashing)
        {
            if (currentMovementCooldown < totalMovementCooldown)
            {
                Debug.Log("move cooldown");
                currentMovementCooldown += Time.deltaTime;                
            }
            else
            {
                canRotate = true;
                canDash = false;
                currentMovementCooldown = 0;
            }
        }
        else
        {
            if (canRotate)
                RotateTowardTarget();
            else if(canDash)
                DashToTarget();
        }
        
    }

    private void RotateTowardTarget()
    {
        if (currentRotateCooldown / totalRotateCooldown >= 1)
        {
            currentRotateCooldown = 0;
            Debug.Log("facing target reached");
            canDash = true;
            canRotate = false;
            return;
        }

        Debug.Log("rotate to target");
        Vector2 aimDirection = (targetTransform.position - transform.position).normalized;
        float angleZ = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        currentRotateCooldown += Time.deltaTime;
        bossRb.rotation = Mathf.Lerp(bossRb.rotation, angleZ, currentRotateCooldown/ totalRotateCooldown);        


    }

    private void DashToTarget()
    {
        Debug.Log("Dashing");
        StartCoroutine(DashCoroutine());
    }

    IEnumerator DashCoroutine()
    {
        canDash = false;
        IsStopped = true;
        currentDashCooldown = 0;
        Debug.Log("Wait to Dash");
        while (currentDashCooldown < totalDashCooldown)
        {
            currentDashCooldown += Time.deltaTime;            
            yield return null;
        }
        IsStopped = false;
        isDashing = true;
        onStartDash?.Invoke();
        bossRb.velocity = transform.right * dashSpeed; // Dash in the direction that was held down.
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        onEndDash?.Invoke();
        bossRb.velocity = Vector2.zero;
    }
    

}
