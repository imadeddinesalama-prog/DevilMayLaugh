using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 14f;

    [Header("Jump Feel")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Glide")]
    public float glideGravity = 0.3f;
    public float glideHorizontalSpeed = 4f;

    [Header("Ground Angle")]
    public float maxGroundAngle = 46f;

    [Header("Audio")]
    public AudioClip jumpSound;
    [Range(0f, 1f)] public float jumpVolume = 1f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private AudioSource audioSource;
    private bool isDead = false;
    private int groundContactCount = 0;
    public bool isGrounded => groundContactCount > 0;

    private bool canGlide = false;
    private bool isGliding = false;
    private bool isMirrored = false;
    private bool isGravityFlipped = false;  // ← new

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDead) return;

        float moveInput = Input.GetAxisRaw("Horizontal");
        if (isMirrored) moveInput = -moveInput;

        // Detect if gravity is flipped
        isGravityFlipped = rb.gravityScale < 0;

        // ── Glide ─────────────────────────────────────────────────────
        isGliding = canGlide
                    && Input.GetButton("Jump")
                    && !isGrounded
                    && (isGravityFlipped
                        ? rb.linearVelocity.y > 0   // flipped: falling = going up
                        : rb.linearVelocity.y < 0);

        if (isGliding)
        {
            rb.linearVelocity = new Vector2(
                moveInput * glideHorizontalSpeed,
                isGravityFlipped ? glideGravity : -glideGravity
            );
        }
        else
        {
            rb.linearVelocity = new Vector2(
                moveInput * moveSpeed, rb.linearVelocity.y);

            // Jump feel only in normal gravity
            if (!isGravityFlipped)
            {
                if (rb.linearVelocity.y < 0)
                    rb.linearVelocity += Vector2.up * Physics2D.gravity.y
                                         * (fallMultiplier - 1) * Time.deltaTime;
                else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
                    rb.linearVelocity += Vector2.up * Physics2D.gravity.y
                                         * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }

        // ── Flip sprite ───────────────────────────────────────────────
        if (moveInput > 0) sr.flipX = false;
        else if (moveInput < 0) sr.flipX = true;

        // ── Flip sprite Y when gravity flipped ────────────────────────
        sr.flipY = isGravityFlipped;

        // ── Jump ──────────────────────────────────────────────────────
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            float force = isGravityFlipped ? -jumpForce : jumpForce;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);

            if (jumpSound != null)
            {
                if (audioSource != null)
                    audioSource.PlayOneShot(jumpSound, jumpVolume);
                else
                    AudioSource.PlayClipAtPoint(
                        jumpSound, transform.position, jumpVolume);
            }
        }

        // ── Animations ────────────────────────────────────────────────
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(moveInput));
            anim.SetBool("isJumping", !isGrounded && !isGliding);
            anim.SetBool("isGliding", isGliding);
        }
    }

    public void SetMirrored(bool value) { isMirrored = value; }

    public void SetGliding(bool value)
    {
        canGlide = value;
        if (!value)
        {
            isGliding = false;
            if (anim != null) anim.SetBool("isGliding", false);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
            UpdateGroundCount();
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
            UpdateGroundCount();
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            groundContactCount = 0;
            Invoke(nameof(UpdateGroundCount), 0.05f);
        }
    }

    void UpdateGroundCount()
    {
        groundContactCount = 0;
        ContactPoint2D[] contacts = new ContactPoint2D[10];
        int count = rb.GetContacts(contacts);

        for (int i = 0; i < count; i++)
        {
            Vector2 referenceNormal = isGravityFlipped ? Vector2.down : Vector2.up;
            float angle = Vector2.Angle(contacts[i].normal, referenceNormal);
            if (angle < maxGroundAngle)
                groundContactCount++;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Deadly"))
            GetComponent<DeathHandler>().Die();
    }

    public void SetDead(bool value)
    {
        isDead = value;
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.gravityScale = 4f;       // reset gravity on death
            isMirrored = false;
            if (anim != null)
            {
                anim.SetFloat("Speed", 0f);
                anim.SetBool("isJumping", false);
                anim.SetBool("isGliding", false);
            }
        }
    }
}