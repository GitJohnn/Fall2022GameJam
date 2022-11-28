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
    public bool useSmoothDamp = false;
    Vector3 velocity = Vector3.zero;
    Vector3 targetPos;
    // Use this for initialization
    void Awake()
    {
        if (!target)
            target = GameObject.FindObjectOfType<PlayerMovement>();
        targetPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posNoZ);

            interpVelocity = targetDirection.magnitude * 5f;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            if (!useSmoothDamp) transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.7f);
            else transform.position = Vector3.SmoothDamp(transform.position, targetPos + offset, ref velocity, 0.05f);
            

        }
    }
}
