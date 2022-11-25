using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : PickupBase
{
    [SerializeField] float _HP;
    protected override void Pickup(GameObject player)
    {
        var playerHealth = player.GetComponent<HealthBase>();
        if (playerHealth) playerHealth.AddHealth(_HP); 
        base.Pickup(player);
    }
}
