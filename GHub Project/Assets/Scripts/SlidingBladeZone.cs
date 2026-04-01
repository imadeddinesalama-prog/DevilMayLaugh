using UnityEngine;

public class SlidingBladeZone : MonoBehaviour
{
    public SlidingFloorBlade blade;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            blade?.Activate();
    }
}