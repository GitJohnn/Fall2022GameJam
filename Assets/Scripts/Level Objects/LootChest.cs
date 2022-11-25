using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LootChest : TriggerEventHandler
{
    public KeyCode interactKeycode = KeyCode.I;
    public UnityEvent OnOpenChest;

    public bool CanOpen { get; set; } = true;

    private void Update()
    {
        if (!CanOpen)
            return;

        if(isInsideTrigger && Input.GetKeyDown(interactKeycode))
        {
            Open();
        }
    }

    [Button]
    private void Open()
    {
        // TODO: This will be called by the player when they try to open a chest?
        OnOpenChest?.Invoke();
    }
}
