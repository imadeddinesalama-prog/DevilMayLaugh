using UnityEngine;

public class ProximityPopup : MonoBehaviour
{
    public GameObject popupImage;
    public float displayTime = 2f;

    private bool hasShown = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasShown) return;
        if (!other.CompareTag("Player")) return;

        hasShown = true;
        StartCoroutine(ShowThenHide());
    }

    System.Collections.IEnumerator ShowThenHide()
    {
        if (popupImage != null)
            popupImage.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        if (popupImage != null)
            popupImage.SetActive(false);
    }
}