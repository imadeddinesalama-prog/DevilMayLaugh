using UnityEngine;

public class TrollPlatform : MonoBehaviour
{
    [Header("Detection")]
    public float detectWidth = 2f;
    public float detectHeight = 3f;

    [Header("Slide")]
    public float slideSpeed = 12f;
    public float slideDistance = 5f;
    public bool slideRight = true;

    [Header("Timing")]
    public float returnDelay = 2f;

    private Vector3 originalPosition;
    private bool isSliding = false;
    private bool isReturning = false;
    private bool hasTriggered = false;   

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        if (isSliding || isReturning || hasTriggered) return;

        Vector2 boxCenter = new Vector2(
            transform.position.x,
            transform.position.y + detectHeight / 2f
        );
        Vector2 boxSize = new Vector2(detectWidth, detectHeight);

        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f,
                         LayerMask.GetMask("Player"));

        if (hit != null)
        {
            Rigidbody2D playerRb = hit.GetComponent<Rigidbody2D>();
            if (playerRb != null && playerRb.linearVelocity.y < -0.5f)
            {
                hasTriggered = true;     
                StartCoroutine(SlideAway());
            }
        }
    }

    System.Collections.IEnumerator SlideAway()
    {
        isSliding = true;

        Vector3 targetPos = originalPosition + new Vector3(
            slideRight ? slideDistance : -slideDistance, 0f, 0f
        );

        while (Vector3.Distance(transform.position, targetPos) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, targetPos, slideSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = targetPos;
        isSliding = false;

        yield return new WaitForSeconds(returnDelay);

        isReturning = true;

        while (Vector3.Distance(transform.position, originalPosition) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, originalPosition, slideSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = originalPosition;
        isReturning = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = new Vector3(
            transform.position.x,
            transform.position.y + detectHeight / 2f,
            0
        );
        Gizmos.DrawWireCube(center, new Vector3(detectWidth, detectHeight, 0));
    }
}