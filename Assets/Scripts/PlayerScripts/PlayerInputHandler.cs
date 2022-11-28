using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] UnityEvent _onLeftClick;
    [SerializeField] UnityEvent _onRightClick;
    [SerializeField] UnityEvent _onDashAbility;
    [SerializeField] KeyCode dashAbilityKey;
    [SerializeField] UnityEvent _onBombAbility;
    [SerializeField] KeyCode bombAbilityKey;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        { 
            _onLeftClick?.Invoke(); 
            if (ScreenShakeController.Instance) ScreenShakeController.Instance.StartShake(0.07f, 0.02f); 
        }
        if (Input.GetMouseButtonDown(1)) 
        { 
            _onRightClick?.Invoke(); 
        }
        if (Input.GetKeyDown(dashAbilityKey)) 
        { 
            _onDashAbility?.Invoke(); 
        }
        if (Input.GetKeyDown(bombAbilityKey)) 
        { 
            _onBombAbility?.Invoke(); 
        }
    }

}
