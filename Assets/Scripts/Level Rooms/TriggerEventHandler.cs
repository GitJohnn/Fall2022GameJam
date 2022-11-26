using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventHandler : MonoBehaviour
{
    public UnityEvent OnTriggerEnter;
    public UnityEvent OnTriggerExit;

    protected bool isInsideTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter?.Invoke();
        isInsideTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExit?.Invoke();
        isInsideTrigger = false;
    }
}
