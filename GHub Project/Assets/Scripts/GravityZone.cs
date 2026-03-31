using UnityEngine;

public class GravityZone : MonoBehaviour
{
    [Header("Settings")]
    public float flippedGravityScale = -2f;   
    public float normalGravityScale = 4f;   

    private bool isFlipped = false;
    private PlayerController playerInZone = null;
    private Rigidbody2D playerRb = null;

    public void ActivateFlip()
    {
        isFlipped = true;

        if (playerRb != null)
            ApplyGravity(flippedGravityScale);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInZone = other.GetComponent<PlayerController>();
        playerRb = other.GetComponent<Rigidbody2D>();

        if (isFlipped && playerRb != null)
            ApplyGravity(flippedGravityScale);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (playerRb != null)
            ApplyGravity(normalGravityScale);

        playerInZone = null;
        playerRb = null;
    }

    void ApplyGravity(float scale)
    {
        playerRb.gravityScale = scale;
    }
}