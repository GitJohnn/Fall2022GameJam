using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] UnityEvent _onLeftClick;
    [SerializeField] UnityEvent _onRightClick;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) _onLeftClick?.Invoke();
        if (Input.GetMouseButtonDown(1)) _onRightClick?.Invoke();
    }

}
