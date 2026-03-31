using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EndDoor : MonoBehaviour
{
    public GameObject toBeContinuedUI;
    public float zoomSpeed = 0.5f;
    public float zoomAmount = 1.3f;
    public float delayBeforeShow = 0.8f;

    private bool triggered = false;
    private Camera cam;

    void Awake()
    {
        if (toBeContinuedUI != null)
            toBeContinuedUI.SetActive(false);
    }

    void Start()
    {
        cam = Camera.main;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        other.GetComponent<PlayerController>()?.SetDead(true);

        StartCoroutine(ToBeContinuedSequence());
    }

    IEnumerator ToBeContinuedSequence()
    {
        float elapsed = 0f;
        float startSize = cam.orthographicSize;
        float targetSize = startSize / zoomAmount;

        while (elapsed < delayBeforeShow)
        {
            elapsed += Time.deltaTime;
            cam.orthographicSize = Mathf.Lerp(
                startSize, targetSize, elapsed / delayBeforeShow);
            yield return null;
        }

        if (toBeContinuedUI != null)
        {
            toBeContinuedUI.SetActive(true);
            Debug.Log("ToBeContinued UI activated!");
        }
        else
        {
            Debug.LogWarning("ToBeContinuedUI is NULL — not assigned!");
        }

        yield return new WaitForSecondsRealtime(0.5f); 
        Time.timeScale = 0f;
    }
}
