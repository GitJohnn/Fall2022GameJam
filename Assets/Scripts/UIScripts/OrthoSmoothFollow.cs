using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthoSmoothFollow : MonoBehaviour
{
    private float interpVelocity;
    private float minDistance;
    private float followDistance;
    public PlayerMovement target;
    public Vector3 offset;
    Vector3 targetPos;
    // Use this for initialization
    void Awake()
    {
        if (!target)
            target = GameObject.FindObjectOfType<PlayerMovement>();
        targetPos = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posNoZ);

            interpVelocity = targetDirection.magnitude * 5f;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.5f);

        }
    }
}
