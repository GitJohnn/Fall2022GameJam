using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventHandler : MonoBehaviour
{
    public UnityEvent OnTriggerEnter;
    public UnityEvent OnTriggerExit;

    [SerializeField] LayerMask triggerLayer = 6;

    protected bool isInsideTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if ((triggerLayer & (1 << collision.gameObject.layer)) != 0)
        {
            OnTriggerEnter?.Invoke();
            isInsideTrigger = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            OnTriggerExit?.Invoke();
            isInsideTrigger = false;
        }
            
    }
}
