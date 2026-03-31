using UnityEngine;

public class CrusherPlatform : MonoBehaviour
{
    [Header("Movement")]
    public Vector3 moveDirection = Vector3.right;  // direction to move toward
    public float moveDistance = 2f;
    public float moveSpeed = 4f;

    [Header("Timing")]
    public float pauseAtEnd = 0.5f;    // pause when fully closed
    public float pauseAtStart = 1f;    // pause when fully open

    [Header("Deadly")]
    public bool killOnContact = true;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;

    // Called by CrusherManager to start
    public void Initialize()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + moveDirection.normalized * moveDistance;
    }

    public void StartCrushing()
    {
        if (!isMoving)
            StartCoroutine(CrushLoop());
    }

    System.Collections.IEnumerator CrushLoop()
    {
        isMoving = true;

        while (true)
        {
            // Pause open
            yield return new WaitForSeconds(pauseAtStart);

            // Move toward center
            yield return MoveTo(targetPosition);

            // Pause closed
            yield return new WaitForSeconds(pauseAtEnd);

            // Move back
            yield return MoveTo(originalPosition);
        }
    }

    System.Collections.IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!killOnContact) return;
        if (col.collider.CompareTag("Player"))
            col.collider.GetComponent<DeathHandler>()?.Die();
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (!killOnContact) return;
        if (col.collider.CompareTag("Player"))
            col.collider.GetComponent<DeathHandler>()?.Die();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 target = Application.isPlaying
            ? targetPosition
            : transform.position + moveDirection.normalized * moveDistance;
        Gizmos.DrawLine(transform.position, target);
        Gizmos.DrawWireCube(target, GetComponent<Collider2D>() != null
            ? GetComponent<Collider2D>().bounds.size : Vector3.one);
    }
}