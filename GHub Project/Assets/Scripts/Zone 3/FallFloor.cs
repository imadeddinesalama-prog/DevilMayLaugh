using UnityEngine;

public class FallFloor : MonoBehaviour
{
    [Header("Fall Settings")]
    public float shakeTime = 0.3f;
    public float shakeMagnitude = 0.05f;
    public float fallSpeed = 10f;
    public float fallDistance = 8f;

    private Vector3 originalPosition;
    private bool hasTriggered = false;
    private bool isFalling = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    public void TriggerFall()
    {
        if (hasTriggered) return;
        hasTriggered = true;
        Debug.Log("FallFloor: starting fall sequence");
        StartCoroutine(FallSequence());
    }

    System.Collections.IEnumerator FallSequence()
    {
        // Shake
        float elapsed = 0f;
        while (elapsed < shakeTime)
        {
            transform.position = originalPosition + new Vector3(
                Random.Range(-shakeMagnitude, shakeMagnitude),
                Random.Range(-shakeMagnitude, shakeMagnitude), 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;

        // Fall
        isFalling = true;
        Vector3 targetPos = originalPosition + new Vector3(0f, -fallDistance, 0f);
        Debug.Log("FallFloor: falling to " + targetPos);

        while (Vector3.Distance(transform.position, targetPos) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, targetPos, fallSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isFalling = false;
        Debug.Log("FallFloor: reached bottom");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (isFalling && col.collider.CompareTag("Player"))
            col.collider.GetComponent<DeathHandler>()?.Die();
    }
}