using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public int projectileDamage = 5;
    public float timeToLive = 4f;
    [SerializeField]private LayerMask collisionLayer;

    public UnityEvent _onHitCollision;

    private void Awake()
    {
        StartCoroutine(DestroySelfAferTime(timeToLive));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if((collisionLayer & (1 << other.gameObject.layer)) != 0)
        {
            var targetHealth = other.GetComponent<Health>();
            if (!targetHealth) return;

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
