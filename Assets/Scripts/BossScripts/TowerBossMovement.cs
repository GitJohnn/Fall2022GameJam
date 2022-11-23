using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBossMovement : MonoBehaviour
{
    public Transform[] movePositions;
    public float moveSpeed = 3f;

    private bool isMoving;
    private int currentPosition = 0;
    private float stopDistance = 0.1f;
    private float distance;
    private Vector2 direction;

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, movePositions[currentPosition].position);

        if(distance > 0.1f)
        {
            direction = (movePositions[currentPosition].position - transform.position).normalized;            
        }
        else
        {
            currentPosition = (currentPosition < movePositions.Length) ? currentPosition++ : 0;
        }
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }
}
