using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static void SpawnParticles(GameObject particles, GameObject parent, bool makeChildOfParent)
    {
        if(makeChildOfParent == false)
            Instantiate(particles, parent.transform.position, parent.transform.rotation);
        if (makeChildOfParent == true)
            Instantiate(particles, parent.transform);
    }
}
