using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private SfxReference _pickupSound;

    private void OnTriggerEnter2D(Collider2D col)
    {
        // TODO: Collider is Player
        Pickup();
    }

    [Button]
    private void Pickup()
    {
        _pickupSound.PlayAtPosition(transform.position);
        // TODO: Increase Player Score?
        Destroy(gameObject);
    }
}
