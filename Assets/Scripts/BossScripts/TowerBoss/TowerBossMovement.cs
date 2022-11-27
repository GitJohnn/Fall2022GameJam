using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBossMovement : MonoBehaviour
{
    public Transform towerBossObj;
    public Transform[] movePositions;
    public float moveSpeed = 3f;

    private bool isMoving;
    private int currentPosition = 0;
    private float stopDistance = 0.2f;
    private float distance;
    private Vector2 direction;

    public bool StopMovement { get { return stopMovement; } set { stopMovement = value; } }
    private bool stopMovement = true;

    void Update()
    {
        if (stopMovement || TowerBossScript.isBossDead)
            return;

        distance = Vector3.Distance(towerBossObj.position, movePositions[currentPosition].position);

        if (distance > stopDistance)
        {
            direction = (movePositions[currentPosition].position - towerBossObj.position).normalized;            
        }
        else
        {            
            currentPosition = (currentPosition < movePositions.Length - 1) ? (currentPosition + 1) : 0;

            direction = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if (stopMovement || TowerBossScript.isBossDead)
            return;

        towerBossObj.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }    

}
