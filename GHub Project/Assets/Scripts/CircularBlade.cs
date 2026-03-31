using UnityEngine;

public class CircularBlade : MonoBehaviour
{
    [Header("Rotation")]
    public float rotateSpeed = 360f;

    [Header("Slide Down")]
    public float slideSpeed = 15f;
    public float slideDistance = 5f;

    [Header("Detection Zone")]
    public Vector2 detectSize = new Vector2(2f, 4f);
    public Vector2 detectOffset = new Vector2(0f, -3f);

    private Vector3 originalPosition;
    private bool hasTriggered = false;
    private bool isSliding = false;
    private bool isResetting = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);

        if (hasTriggered || isSliding || isResetting) return;

        Vector2 center = new Vector2(
            transform.position.x + detectOffset.x,
            transform.position.y + detectOffset.y
        );

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, detectSize, 0f);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hasTriggered = true;
                StartCoroutine(SlideDown());
                break;
            }
        }
    }

    System.Collections.IEnumerator SlideDown()
    {
        isSliding = true;

        Vector3 targetPos = originalPosition + new Vector3(0f, -slideDistance, 0f);

        while (Vector3.Distance(transform.position, targetPos) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                slideSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = targetPos;
        isSliding = false;

        yield return new WaitForSeconds(0.3f);

        isResetting = true;

        while (Vector3.Distance(transform.position, originalPosition) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                originalPosition,
                slideSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = originalPosition;
        isResetting = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<DeathHandler>()?.Die();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<DeathHandler>()?.Die();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(
            new Vector3(
                transform.position.x + detectOffset.x,
                transform.position.y + detectOffset.y, 0),
            new Vector3(detectSize.x, detectSize.y, 0)
        );
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            transform.position + new Vector3(0, -slideDistance, 0),
            Vector3.one
        );
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            transform.position,
            transform.position + new Vector3(0, -slideDistance, 0)
        );
    }
}