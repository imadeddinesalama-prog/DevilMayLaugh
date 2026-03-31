using UnityEngine;

public class WallTrapTrigger : MonoBehaviour
{
    public MovingWallTrap wall;  

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            wall?.OnPlayerDetected();
    }
}