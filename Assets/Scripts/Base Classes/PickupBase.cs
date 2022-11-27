using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SoundSystem;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PickupBase : MonoBehaviour
{
    [SerializeField] GameObject _particleOnPickup;
    [SerializeField] SFXEvent _soundOnPickup;
    [SerializeField] UnityEvent _onPickupEvent;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] float speed = 5f;
    [SerializeField] float height = 0.1f;

    void OnValidate()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerInputHandler>()) Pickup(other.gameObject);
    }

    protected virtual void Pickup(GameObject player)
    {
        PickupFeedback();
        Destroy(gameObject);
    }

    protected virtual void PickupFeedback()
    {
        if (_particleOnPickup) Utility.SpawnParticles(_particleOnPickup, gameObject, false);
        if (_soundOnPickup != null) _soundOnPickup.Play();
        _onPickupEvent?.Invoke();
    }
}
