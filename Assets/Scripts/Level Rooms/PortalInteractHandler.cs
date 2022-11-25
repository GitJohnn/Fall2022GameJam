using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalInteractHandler : TriggerEventHandler
{
    public KeyCode interactKeycode = KeyCode.I;
    public UnityEvent OnInteract;

    public bool CanInteract { get; set; }

    private void Update()
    {
        if (!CanInteract)
            return;

        if(isInsideTrigger && Input.GetKeyDown(interactKeycode))
        {
            OnInteract?.Invoke();
        }
    }

}
