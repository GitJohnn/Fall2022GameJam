using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHealth : HealthBase
{
    [SerializeField] float _maxObstacleHealth;
    public override void ResetHealth()
    {
        InitializeHealth(_maxObstacleHealth);
    }
}
