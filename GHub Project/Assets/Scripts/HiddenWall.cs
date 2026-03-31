using UnityEngine;

public class HiddenWall : MonoBehaviour
{
    [Header("Fade Settings")]
    public float fadeSpeed = 3f;
    public float minAlpha = 0f;        // fully invisible when inside
    public float maxAlpha = 1f;        // fully visible normally
    public float detectionRadius = 1.5f;

    private SpriteRenderer sr;
    private float targetAlpha;
    private Transform player;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        targetAlpha = maxAlpha;

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // Fade out when player is close
        targetAlpha = dist < detectionRadius ? minAlpha : maxAlpha;

        Color c = sr.color;
        c.a = Mathf.Lerp(c.a, targetAlpha, fadeSpeed * Time.deltaTime);
        sr.color = c;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}