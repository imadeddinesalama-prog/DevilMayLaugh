using UnityEngine;

public class Revolver : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 15f;
    public int maxBullets = 6;

    [Header("Audio")]
    public AudioClip shootSound;
    public AudioClip emptySound;
    [Range(0f, 1f)] public float shootVolume = 1f;

    // Static — survives scene reload
    private static int savedBullets = 6;
    private static bool savedHasWeapon = false;

    private int currentBullets = 0;
    private bool hasWeapon = false;
    private Animator anim;
    private AudioSource audioSource;
    private SpriteRenderer sr;

    void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();

        if (savedHasWeapon)
        {
            hasWeapon = true;
            currentBullets = savedBullets;
        }
    }

    void Update()
    {
        if (!hasWeapon) return;

        if (firePoint != null)
        {
            Vector3 fp = firePoint.localPosition;
            fp.x = sr.flipX ? -Mathf.Abs(fp.x) : Mathf.Abs(fp.x);
            firePoint.localPosition = fp;
        }

        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Z))
            TryShoot();
    }

    void TryShoot()
    {
        if (currentBullets <= 0)
        {
            if (emptySound != null)
                AudioSource.PlayClipAtPoint(emptySound, transform.position);
            return;
        }

        currentBullets--;
        savedBullets = currentBullets; 

        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(
                bulletPrefab, firePoint.position, Quaternion.identity);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float direction = sr.flipX ? -1f : 1f;
                rb.linearVelocity = new Vector2(direction * bulletSpeed, 0f);
            }
        }

        if (anim != null) anim.SetTrigger("Shoot");

        if (shootSound != null)
            AudioSource.PlayClipAtPoint(
                shootSound, transform.position, shootVolume);
    }

    public void PickupRevolver()
    {
        hasWeapon = true;
        savedHasWeapon = true;
        currentBullets = maxBullets;
        savedBullets = maxBullets; 
    }

    public int GetBullets() => currentBullets;
    public bool HasWeapon() => hasWeapon;
}