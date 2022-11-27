using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalInteractHandler : TriggerEventHandler
{
    public KeyCode interactKeycode = KeyCode.I;
    public UnityEvent OnInteract;
    public UnityEvent OnScreenFade;

    public bool CanInteract { get; set; } = true;

    private void OnDisable()
    {
        //FadeAnimationScript.OnFade.RemoveListener(OnScreenFadeAction);
        FadeAnimationScript.OnFade -= ScreenFadeEventCall;
    }

    private void Update()
    {
        if (!CanInteract)
            return;

        if(isInsideTrigger && Input.GetKeyDown(interactKeycode))
        {
            OnInteract?.Invoke();
            //FadeAnimationScript.OnFade.AddListener(OnScreenFadeAction);
            FadeAnimationScript.OnFade += ScreenFadeEventCall;
            Debug.Log("Interacting with " + gameObject.name);
            CanInteract = false;
        }
    }

    private void ScreenFadeEventCall()
    {
        OnScreenFade?.Invoke();
        Debug.Log("Calling screen fade event");
    }

}
