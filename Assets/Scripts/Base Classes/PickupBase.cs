using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PickupBase : MonoBehaviour
{
    [SerializeField] GameObject _particleOnPickup;
    [SerializeField] SfxReference _soundOnPickup;
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
        Pickup(other.gameObject);
    }

    protected virtual void Pickup(GameObject player)
    {
        PickupFeedback();
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        Movement();
    }

    protected virtual void Movement()
    {
        

        Vector2 position = transform.position;
        float newYValue = Mathf.Sin(Time.time * speed) + transform.position.y;
        _rb.MovePosition(new Vector2(transform.position.x, newYValue * height));
    }

    protected virtual void PickupFeedback()
    {
        if (_particleOnPickup) Instantiate(_particleOnPickup, transform.position, Quaternion.identity);
        if (_soundOnPickup != null) _soundOnPickup.PlayAtPosition(transform.position);
        _onPickupEvent?.Invoke();
    }
}
