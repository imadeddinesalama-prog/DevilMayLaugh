using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Settings")]
    public AudioClip collectSound;
    [Range(0f, 1f)] public float volume = 1f;

    private bool collected = false;

    void Start()
    {
        // If already collected before death — stay gone
        string coinID = transform.position.x + "_" + transform.position.y;
        if (CollectedCoins.ids.Contains(coinID))
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        if (!other.CompareTag("Player")) return;

        collected = true;

        string coinID = transform.position.x + "_" + transform.position.y;
        CollectedCoins.ids.Add(coinID);

        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position, volume);

        Destroy(gameObject);
    }
}

// Persists across scene reloads
public static class CollectedCoins
{
    public static System.Collections.Generic.HashSet<string> ids
        = new System.Collections.Generic.HashSet<string>();
}