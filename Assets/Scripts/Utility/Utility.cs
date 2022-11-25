using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static void SpawnParticles(GameObject particles, GameObject parent)
    {
        Instantiate(particles, parent.transform.position, parent.transform.rotation);
    }
}
