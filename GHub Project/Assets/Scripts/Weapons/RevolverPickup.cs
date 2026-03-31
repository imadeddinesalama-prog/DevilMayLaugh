using UnityEngine;

public class RevolverPickup : MonoBehaviour
{
    public WeaponUnlockPopup popup;     // drag popup here
    public AudioClip pickupSound;
    [Range(0f, 1f)] public float volume = 1f;

    private static bool alreadyPickedUp = false;

    void Start()
    {
        if (alreadyPickedUp)
        {
            Revolver rev = FindFirstObjectByType<Revolver>();
            if (rev != null) rev.PickupRevolver();
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        alreadyPickedUp = true;

        Revolver rev = other.GetComponent<Revolver>();
        if (rev != null) rev.PickupRevolver();

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume);

        // Show popup
        if (popup != null) popup.Show();

        Destroy(gameObject);
    }
}