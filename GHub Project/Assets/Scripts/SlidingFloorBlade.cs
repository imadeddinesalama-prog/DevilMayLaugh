using UnityEngine;

public class SlidingFloorBlade : MonoBehaviour
{
    [Header("Slide Up")]
    public float riseSpeed = 15f;
    public float riseDistance = 2f;

    [Header("Slide Left")]
    public float slideSpeed = 6f;
    public float slideDistance = 15f;

    [Header("Rotation")]
    public float rotateSpeed = 360f;

    private Vector3 originalPosition;
    private bool hasTriggered = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    public void Activate()
    {
        if (hasTriggered) return;
        hasTriggered = true;
        StartCoroutine(RiseThenSlide());
    }

    System.Collections.IEnumerator RiseThenSlide()
    {
        // Rise up
        Vector3 riseTarget = originalPosition + new Vector3(0f, riseDistance, 0f);
        while (Vector3.Distance(transform.position, riseTarget) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, riseTarget, riseSpeed * Time.deltaTime);
            transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = riseTarget;

        // Slide left
        Vector3 slideTarget = transform.position + new Vector3(-slideDistance, 0f, 0f);
        while (Vector3.Distance(transform.position, slideTarget) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, slideTarget, slideSpeed * Time.deltaTime);
            transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = slideTarget;
        Destroy(gameObject, 0.5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<DeathHandler>()?.Die();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<DeathHandler>()?.Die();
    }
}