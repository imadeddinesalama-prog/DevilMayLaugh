using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathHandler : MonoBehaviour
{
    private static Vector3 spawnPoint;
    private static bool spawnSet = false;

    [Header("Audio")]
    public AudioClip deathSound;
    [Range(0f, 1f)] public float deathVolume = 1f;

    private bool isDead = false;

    void Start()
    {
        if (!spawnSet)
        {
            spawnPoint = transform.position;
            spawnSet = true;
        }

        transform.position = spawnPoint;
        isDead = false;
    }

    public void SetSpawnPoint(Vector3 newPoint)
    {
        spawnPoint = newPoint;
        Debug.Log("Spawn point updated to: " + newPoint);
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, transform.position, deathVolume);

        Invoke(nameof(ReloadScene), 0.2f);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}