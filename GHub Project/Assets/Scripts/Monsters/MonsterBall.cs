using UnityEngine;

public class MonsterBall : MonoBehaviour
{
    [Header("Detection")]
    public float detectionRange = 6f;

    [Header("Chase")]
    public float chaseSpeed = 5f;
    public float rotateSpeed = 400f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private Transform player;

    private bool isChasing = false;
    private bool isDead = false;

    private string enemyID;
    private static System.Collections.Generic.HashSet<string> killedEnemies
        = new System.Collections.Generic.HashSet<string>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        enemyID = transform.position.x + "_" + transform.position.y;

        if (killedEnemies.Contains(enemyID))
        {
            Destroy(gameObject);
            return;
        }

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (!isChasing && dist <= detectionRange)
        {
            isChasing = true;
            rb.bodyType = RigidbodyType2D.Dynamic;

            if (anim != null)
                anim.SetBool("isRolling", true);
        }

        if (isChasing && !isDead)
            Chase();
    }

    void Chase()
    {
        float dir = player.position.x > transform.position.x ? 1f : -1f;
        rb.linearVelocity = new Vector2(dir * chaseSpeed, rb.linearVelocity.y);
        sr.flipX = dir < 0;
        transform.Rotate(0f, 0f, -dir * rotateSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (isDead) return;
        if (col.collider.CompareTag("Player"))
            col.collider.GetComponent<DeathHandler>()?.Die();
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (isDead) return;
        if (col.collider.CompareTag("Player"))
            col.collider.GetComponent<DeathHandler>()?.Die();
    }

    public void TakeDamage()
    {
        if (isDead) return;
        isDead = true;

        killedEnemies.Add(enemyID);

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        isChasing = false;

        if (anim != null)
        {
            anim.SetBool("isRolling", false);
            anim.SetTrigger("Die");
        }

        Destroy(gameObject, 0.6f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}