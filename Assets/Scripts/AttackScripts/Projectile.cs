using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public int projectileDamage = 5;
    public float timeToLive = 4f;
    [SerializeField]private LayerMask hitLayer, obstacleLayer;

    public UnityEvent _onHitCollision;

    private void Awake()
    {
        StartCoroutine(DestroySelfAferTime(timeToLive));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        LayerMask objlayer = other.gameObject.layer;
        //Debug.Log(objlayer);
        //Check if colliding with obstacle object
        if ((obstacleLayer & (1 << objlayer)) != 0)
        {
            //Debug.Log("hit obstacle layer");
            Destroy(gameObject);
            return;
        }
        //Check if colliding with hit object
        if ((hitLayer & (1 << objlayer)) != 0)
        {
            var targetHealth = other.GetComponent<HealthBase>();
            if (!targetHealth)
                return;
                

            _onHitCollision?.Invoke();
            //Debug.Log(other.gameObject);
            targetHealth.GetHit(projectileDamage, gameObject);
            Destroy(gameObject);
        }
    }

    IEnumerator DestroySelfAferTime(float value)
    {
         yield return new WaitForSeconds(value);
        Destroy(gameObject);

    }
}
