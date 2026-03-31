using UnityEngine;
using System.Collections;

public class JumpBladeTrap : MonoBehaviour
{
    [Header("Rotation")]
    public float rotateSpeed = 500f;

    [Header("Rise")]
    public float riseSpeed = 10f;
    public float riseDistance = 2f;

    [Header("Dash Left")]
    public float dashSpeed = 16f;
    public float dashDistance = 6f;

    private Vector3 startPos;
    private Vector3 upPos;
    private Vector3 endPos;

    private bool hasTriggered = false;
    private bool isRunning = false;

    void Start()
    {
        startPos = transform.position;
        upPos = startPos + Vector3.up * riseDistance;
        endPos = upPos + Vector3.left * dashDistance;
    }

    void Update()
    {
        transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }

    public void TriggerTrap()
    {
        if (hasTriggered || isRunning) return;
        StartCoroutine(TrapRoutine());
    }

    IEnumerator TrapRoutine()
    {
        hasTriggered = true;
        isRunning = true;

        while (Vector3.Distance(transform.position, upPos) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                upPos,
                riseSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = upPos;

        while (Vector3.Distance(transform.position, endPos) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                endPos,
                dashSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = endPos;
        isRunning = false;
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