using UnityEngine;

public class BlockingPlatform : MonoBehaviour
{
    [Header("Detection Zone (to the side)")]
    public float detectWidth = 3f;
    public float detectHeight = 2f;
    public bool detectFromRight = false; 

    [Header("Move Up")]
    public float moveSpeed = 15f;
    public float moveDistance = 4f;

    [Header("Timing")]
    public float returnDelay = 3f;

    private Vector3 originalPosition;
    private bool isMoving = false;
    private bool isReturning = false;
    private bool hasTriggered = false;

    private BoxCollider2D boxCol;

    void Start()
    {
        originalPosition = transform.position;
        boxCol = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (isMoving || isReturning || hasTriggered) return;

        Collider2D hit = Physics2D.OverlapBox(GetDetectionCenter(),
                         new Vector2(detectWidth, detectHeight), 0f,
                         LayerMask.GetMask("Player"));

        if (hit != null)
        {
            hasTriggered = true;
            StartCoroutine(MoveUp());
        }
    }
    Vector2 GetDetectionCenter()
    {
        float platformHalfWidth = boxCol != null ? boxCol.bounds.size.x / 2f : 0.5f;
        float direction = detectFromRight ? 1f : -1f;

        return new Vector2(
            transform.position.x + direction * (platformHalfWidth + detectWidth / 2f),
            transform.position.y   
        );
    }

    System.Collections.IEnumerator MoveUp()
    {
        isMoving = true;

        Vector3 targetPos = originalPosition + new Vector3(0f, moveDistance, 0f);

        while (Vector3.Distance(transform.position, targetPos) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;

        yield return new WaitForSeconds(returnDelay);

        isReturning = true;

        while (Vector3.Distance(transform.position, originalPosition) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                originalPosition,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = originalPosition;
        isReturning = false;
    }

    void OnDrawGizmosSelected()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(
            new Vector3(GetDetectionCenter().x, GetDetectionCenter().y, 0),
            new Vector3(detectWidth, detectHeight, 0)
        );

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(
            transform.position + new Vector3(0, moveDistance, 0),
            col.bounds.size
        );

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, moveDistance, 0));
    }
}