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

    [SerializeField] GameObject _vfxUsePortal;

    private void OnDisable()
    {
        //FadeAnimationScript.OnFade.RemoveListener(OnScreenFadeAction);
        FadeAnimationScript.OnFaded -= (ScreenFadeEventCall);
    }

    private void Update()
    {
        if (!CanInteract)
            return;

        if(isInsideTrigger && Input.GetKeyDown(interactKeycode))
        {
            Utility.SpawnParticles(_vfxUsePortal, this.gameObject, false);
            OnInteract?.Invoke();
            //FadeAnimationScript.OnFade.AddListener(OnScreenFadeAction);
            FadeAnimationScript.OnFaded += (ScreenFadeEventCall);
            //Debug.Log("Interacting with " + gameObject.name);
            CanInteract = false;
        }
    }

    private void ScreenFadeEventCall()
    {
        OnScreenFade?.Invoke();
        //Debug.Log("Calling screen fade event");
    }

}
