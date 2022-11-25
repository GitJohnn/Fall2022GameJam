using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalInteractHandler : TriggerEventHandler
{
    public KeyCode interactKeycode = KeyCode.I;
    public UnityEvent OnInteract;

    private void Update()
    {
        if(isInsideTrigger && Input.GetKeyDown(interactKeycode))
        {
            OnInteract?.Invoke();
        }
    }

}
