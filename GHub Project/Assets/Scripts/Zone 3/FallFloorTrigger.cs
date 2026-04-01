using System.Drawing;
using UnityEngine;

public class FallFloorTrigger : MonoBehaviour
{
    public FallFloor fallFloor;
    private bool triggered = false;

    void OnTriggerStay2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc == null) return;

        // Fire when player is moving upward OR is in air
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        bool isInAir = !pc.isGrounded && rb != null;

        if (isInAir)
        {
            triggered = true;
            Debug.Log("Floor falling!");
            fallFloor?.TriggerFall();
        }
    }
}
