using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public float timeToExplode = 2;
    public float timeToFinishExplotion = 1;
    public float explotionRange = 3f;
    public int damageAmount = 25;
    [SerializeField] private LayerMask targetLayers;

    private float currentExplodeTime;

    void Awake()
    {
        StartCoroutine(BombExplotion());
    }

    IEnumerator BombExplotion()
    {
        while(currentExplodeTime <= timeToExplode)
        {
            currentExplodeTime += Time.deltaTime;

            yield return null;
        }

        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, explotionRange, targetLayers);
        foreach (Collider2D collider in targets)
        {
            // Debug.Log(collider.name); 
            HandleHitLogic(collider);
        }
        Debug.Log("Bomb Explosion!");

        yield return new WaitForSeconds(timeToFinishExplotion);

        Destroy(gameObject);
    }

    public virtual void HandleHitLogic(Collider2D collider)
    {
        var targetHealth = collider.GetComponent<HealthBase>();
        if (!targetHealth) return;

        targetHealth.GetHit(damageAmount, gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explotionRange);
    }
}
