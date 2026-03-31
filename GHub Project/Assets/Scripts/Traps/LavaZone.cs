using UnityEngine;

public class LavaZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        DeathHandler death = other.GetComponent<DeathHandler>();
        if (death != null)
            death.Die();
    }
}