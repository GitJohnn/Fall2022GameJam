using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBossMovement : MonoBehaviour
{
    public Transform targetTransform;
    public float totalMovementCooldown = 1.25f;
    public float rotationSpeed = 15f;

    private float currentMovementCooldown = 0;
    private bool canRotate = false;
    private bool canDash = false;
    private bool reachedRotatePos = false;
    private bool reachedMovePos = false;

    private void Update()
    {
        if (!canRotate && !canDash)
        {
            if (currentMovementCooldown < totalMovementCooldown)
            {
                Debug.Log("move cooldown");
                currentMovementCooldown += Time.deltaTime;                
            }
            else
            {
                canRotate = true;
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
        Debug.Log("rotate to target");
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetTransform.position - transform.position), rotationSpeed * Time.deltaTime);
        float angleDifference = Vector2.Angle(transform.position, targetTransform.position);
        Debug.Log("Angle difference is " + angleDifference);
        if (angleDifference < 3f)
        {
            Debug.Log("facing target reached");
            canRotate = false;
            canDash = true;
        }
    }

    private void DashToTarget()
    {
        Debug.Log("Dashing");
        StartCoroutine(DashCoroutine());
    }

    IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(2f);
        canDash = false;
        canRotate = true;
    }
    

}
