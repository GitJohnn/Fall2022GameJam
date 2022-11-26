using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class InvisibleTrigger : MonoBehaviour
{
    [SerializeField, ReadOnly] BoxCollider2D _collider;
    [SerializeField] UnityEvent _OnPlayerEnter; 
    [SerializeField] bool _onlyCalledOnce;

    bool _activatedFlag;

    void Awake()
    {
        _activatedFlag = false;
    }

    void OnValidate()
    {
        if (_collider == null) _collider = GetComponent<BoxCollider2D>();
        if (!_collider.isTrigger) Debug.LogWarning("BoxCollider needs to be a Trigger");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_activatedFlag && _onlyCalledOnce) return;

        var player = other.GetComponent<PlayerInputHandler>();
        if (player)
        {
            OnPlayerTrigger(player.gameObject);
            _activatedFlag = true;
            _OnPlayerEnter?.Invoke();
        }
    }

    protected virtual void OnPlayerTrigger(GameObject player)
    {

    }
}
