using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupBase : MonoBehaviour
{
    [SerializeField] GameObject _particleOnPickup;
    [SerializeField] SfxReference _soundOnPickup;
    [SerializeField] UnityEvent _onPickupEvent;

    void OnTriggerEnter2D(Collider2D other)
    {
        Pickup(other.gameObject);
    }

    protected virtual void Pickup(GameObject player)
    {
        PickupFeedback();
        Destroy(gameObject);
    }

    protected virtual void PickupFeedback()
    {
        if (_particleOnPickup) Instantiate(_particleOnPickup, transform.position, Quaternion.identity);
        if (_soundOnPickup != null) _soundOnPickup.PlayAtPosition(transform.position);
        _onPickupEvent?.Invoke();
    }
}
