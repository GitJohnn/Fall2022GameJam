using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBossMovement : MonoBehaviour
{
    public Transform bossTransform;
    public Transform playerTransfrom;
    public Transform[] movePositions;
    public float moveSpeed = 3f;

    private bool isMoving;
    private int currentPosition = 0;
    private float stopDistance = 0.1f;
    private float distance;
    private Vector2 direction;

    public bool StopMovement { get { return stopMovement; } set { stopMovement = value; } }
    public bool stopMovement = true;

    public bool StopRotate { get { return stopRotate; } set { stopRotate = value; } }
    public bool stopRotate = true;

    // Update is called once per frame
    void Update()
    {
        if (ProjectileBossScipt.IsBossDead)
            return;

        if (stopMovement)
            return;


        distance = Vector3.Distance(bossTransform.position, movePositions[currentPosition].position);

        if (distance > stopDistance)
        {
            direction = (movePositions[currentPosition].position - bossTransform.position).normalized;
        }
        else
        {
            //Debug.Log(movePositions.Length);
            currentPosition = (currentPosition < movePositions.Length - 1) ? (currentPosition + 1) : 0;
            //Debug.Log(currentPosition);
            direction = Vector3.zero;
        }

        if (StopRotate)
            return;

        RotateToTarget();
    }

    private void RotateToTarget()
    {
        Vector2 aimDirection = (playerTransfrom.position - bossTransform.position).normalized;
        float angleZ = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        bossTransform.rotation = Quaternion.Euler(0, 0, angleZ + 90);
    }

    private void FixedUpdate()
    {
        if (stopMovement)
            return;
        Debug.Log("Move proj boss");
        bossTransform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }
}
