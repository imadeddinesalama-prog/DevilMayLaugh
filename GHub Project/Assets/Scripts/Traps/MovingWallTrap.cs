using UnityEngine;

public class MovingWallTrap : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float moveDistance = 10f;

    [Header("Return")]
    public float returnDelay = 2f;
    public float returnSpeed = 4f;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool hasTriggered = false;
    private bool isMoving = false;
    private bool isReturning = false;

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + new Vector3(-moveDistance, 0f, 0f);
        hasTriggered = false;
        isMoving = false;
        isReturning = false;
        transform.position = originalPosition;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
                StartCoroutine(ReturnAfterDelay());
            }
        }

        if (isReturning)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, originalPosition, returnSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, originalPosition) < 0.01f)
            {
                transform.position = originalPosition;
                isReturning = false;
            }
        }
    }

    System.Collections.IEnumerator ReturnAfterDelay()
    {
        yield return new WaitForSeconds(returnDelay);
        isReturning = true;
    }

    public void OnPlayerDetected()
    {
        if (hasTriggered) return;
        hasTriggered = true;
        isMoving = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
            col.collider.GetComponent<DeathHandler>()?.Die();
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
            col.collider.GetComponent<DeathHandler>()?.Die();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,
            transform.position + new Vector3(-moveDistance, 0f, 0f));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(
            transform.position + new Vector3(-moveDistance, 0f, 0f),
            GetComponent<Collider2D>() != null
                ? GetComponent<Collider2D>().bounds.size : Vector3.one);
    }
}