using UnityEngine;

public class SequentialFallingPlatform : MonoBehaviour
{
    [Header("Fall Settings")]
    public float fallSpeed = 8f;
    public float fallDistance = 6f;
    public float returnDelay = 3f;
    public float returnSpeed = 4f;

    private Vector3 originalPosition;
    private bool isFalling = false;
    private bool isReturning = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    public void TriggerFall()
    {
        if (isFalling || isReturning) return;
        StartCoroutine(FallSequence());
    }

    public void Reset()
    {
        StopAllCoroutines();
        isFalling = false;
        isReturning = false;
        transform.position = originalPosition;
    }

    System.Collections.IEnumerator FallSequence()
    {
        isFalling = true;

        float shakeTime = 0.3f;
        float elapsed = 0f;
        while (elapsed < shakeTime)
        {
            float ox = Random.Range(-0.05f, 0.05f);
            float oy = Random.Range(-0.05f, 0.05f);
            transform.position = originalPosition + new Vector3(ox, oy, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;

        Vector3 targetPos = originalPosition + new Vector3(0f, -fallDistance, 0f);
        while (Vector3.Distance(transform.position, targetPos) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                fallSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = targetPos;
        isFalling = false;

        yield return new WaitForSeconds(returnDelay);

        isReturning = true;
        while (Vector3.Distance(transform.position, originalPosition) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                originalPosition,
                returnSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = originalPosition;
        isReturning = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!isFalling) return; 

        if (col.collider.CompareTag("Player"))
        {
            col.collider.GetComponent<DeathHandler>()?.Die();
        }
    }
}