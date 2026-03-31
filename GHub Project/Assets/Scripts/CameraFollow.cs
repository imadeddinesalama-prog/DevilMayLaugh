using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Follow Settings")]
    public float smoothSpeed = 5f;
    public Vector2 baseOffset = new Vector2(0f, 1f);  // base vertical offset

    [Header("Look Ahead")]
    public float lookAheadDistance = 3f;  // how far ahead the camera looks
    public float lookAheadSpeed = 4f;     // how fast it shifts

    [Header("Lock Axes")]
    public bool lockX = false;
    public bool lockY = false;

    private Vector3 initialPosition;
    private float currentLookAhead = 0f;
    private SpriteRenderer playerSprite;

    void Start()
    {
        initialPosition = transform.position;

        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null)
            {
                player = p.transform;
                playerSprite = p.GetComponent<SpriteRenderer>();
            }
            else
                Debug.LogWarning("CameraFollow: Player not found!");
        }
        else
        {
            playerSprite = player.GetComponent<SpriteRenderer>();
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Determine look-ahead direction from sprite flip
        float targetLookAhead = 0f;
        if (playerSprite != null)
            targetLookAhead = playerSprite.flipX
                ? -lookAheadDistance   // facing left → look left
                : lookAheadDistance;  // facing right → look right

        // Smooth the look-ahead transition
        currentLookAhead = Mathf.Lerp(
            currentLookAhead, targetLookAhead, lookAheadSpeed * Time.deltaTime);

        float targetX = lockX
            ? initialPosition.x
            : player.position.x + baseOffset.x + currentLookAhead;

        float targetY = lockY
            ? initialPosition.y
            : player.position.y + baseOffset.y;

        Vector3 targetPos = new Vector3(targetX, targetY, transform.position.z);

        transform.position = Vector3.Lerp(
            transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}