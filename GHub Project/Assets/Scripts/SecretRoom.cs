using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretRoom : MonoBehaviour
{
    [Header("Fade")]
    public float fadeInSpeed = 4f;
    public float fadeOutSpeed = 2f;

    private TilemapRenderer[] tilemapRenderers;
    private Tilemap[] tilemaps;
    private SpriteRenderer[] spriteRenderers;

    private bool playerInside = false;
    private float currentAlpha = 0f;

    void Awake()
    {
        // Get everything — including deeply nested
        tilemapRenderers = GetComponentsInChildren<TilemapRenderer>(true);
        tilemaps = GetComponentsInChildren<Tilemap>(true);
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);

        // Force hide immediately on first frame
        SetAlpha(0f);
        currentAlpha = 0f;
    }

    void Update()
    {
        float target = playerInside ? 1f : 0f;
        float speed = playerInside ? fadeInSpeed : fadeOutSpeed;

        currentAlpha = Mathf.Lerp(currentAlpha, target, speed * Time.deltaTime);
        SetAlpha(currentAlpha);
    }

    void SetAlpha(float a)
    {
        // Hide Tilemaps via Tilemap.color
        if (tilemaps != null)
            foreach (Tilemap tm in tilemaps)
            {
                if (tm == null) continue;
                Color c = tm.color;
                c.a = a;
                tm.color = c;
            }

        // Hide any SpriteRenderers too
        if (spriteRenderers != null)
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                if (sr == null) continue;
                Color c = sr.color;
                c.a = a;
                sr.color = c;
            }
    }

    public void PlayerEntered()
    {
        playerInside = true;
    }

    public void PlayerExited()
    {
        playerInside = false;
    }
}