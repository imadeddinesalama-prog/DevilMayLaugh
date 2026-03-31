using UnityEngine;
using System.Collections;

public class EscapeDoorMover : MonoBehaviour
{
    public float slideSpeed = 6f;
    public Vector2 moveDirection = Vector2.left;
    public float slideDistance = 4f;

    private Vector3 startPos;
    private Vector3 finalPos;
    private bool isMoving = false;
    private bool hasMoved = false;

    void Start()
    {
        startPos = transform.position;
        finalPos = startPos + (Vector3)(moveDirection.normalized * slideDistance);
    }

    public void MoveDoor()
    {
        if (hasMoved || isMoving) return;
        StartCoroutine(SlideDoor());
    }

    IEnumerator SlideDoor()
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, finalPos) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                finalPos,
                slideSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = finalPos;
        isMoving = false;
        hasMoved = true;
    }

    public Vector3 GetFinalPosition()
    {
        return finalPos;
    }
}