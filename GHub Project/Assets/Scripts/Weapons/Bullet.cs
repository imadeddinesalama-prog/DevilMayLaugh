using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float maxDistance = 8f;

    private Rigidbody2D rb;
    private Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Collider2D playerCol = player.GetComponent<Collider2D>();
            Collider2D bulletCol = GetComponent<Collider2D>();
            if (playerCol != null && bulletCol != null)
                Physics2D.IgnoreCollision(bulletCol, playerCol);
        }
    }

    void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return;

        if (other.CompareTag("Enemy"))
        {
            other.GetComponentInParent<MonsterBall>()?.TakeDamage();
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Flippable"))
        {
            other.GetComponent<GravityFlipObject>()?.TakeHit();
            Destroy(gameObject);
            return;
        }

        if (!other.isTrigger)
            Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player")) return;
        Destroy(gameObject);
    }
}