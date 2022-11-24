using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBossAttack : MonoBehaviour
{
    public Rigidbody2D projectilePrefab;
    public Transform firstTurret;
    public Transform secondTurret;
    public float projectileSpeed = 8;
    public float shootingCooldown;

    public bool StopAttacking { get; set; }

    private int currentTurret = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (StopAttacking)
            return;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
