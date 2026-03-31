using UnityEngine;
using UnityEngine.UI;

public class WeaponUnlockPopup : MonoBehaviour
{
    // No extra references needed
    // Just make sure RawImage is a child of this GameObject

    private bool isShowing = false;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        isShowing = true;
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (!isShowing) return;

        if (Input.anyKeyDown)
            Hide();
    }

    void Hide()
    {
        isShowing = false;
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}