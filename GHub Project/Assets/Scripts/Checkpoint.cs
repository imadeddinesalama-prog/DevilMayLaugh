using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private static HashSet<string> activatedCheckpoints = new HashSet<string>();
    private Animator anim;
    private bool isActivated = false;
    private string checkpointID;

    [Header("Floor Blade to arm")]
    public FloorBlade floorBladeToArm;

    [Header("Audio")]
    public AudioClip activationSound;
    [Range(0f, 1f)] public float volume = 1f;

    void Awake()
    {
        anim = GetComponent<Animator>();
        checkpointID = transform.position.x + "_" + transform.position.y;

        if (activatedCheckpoints.Contains(checkpointID))
        {
            isActivated = true;
            if (anim != null)
            {
                anim.enabled = true;
                anim.Play("CheckpointActivate", 0, 1f);
                anim.speed = 0f;
            }
            if (floorBladeToArm != null) floorBladeToArm.Arm();
        }
        else
        {
            if (anim != null) anim.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isActivated) return;
        if (!other.CompareTag("Player")) return;

        isActivated = true;
        activatedCheckpoints.Add(checkpointID);

        other.GetComponent<DeathHandler>()?.SetSpawnPoint(transform.position);

        if (anim != null)
        {
            anim.enabled = true;
            anim.speed = 1f;
            anim.Play("CheckpointActivate", 0, 0f);
            StartCoroutine(FreezeOnLastFrame());
        }

        if (activationSound != null)
            AudioSource.PlayClipAtPoint(activationSound, transform.position, volume);

        if (floorBladeToArm != null) floorBladeToArm.Arm();
    }

    System.Collections.IEnumerator FreezeOnLastFrame()
    {
        yield return new WaitForSeconds(0.6f);
        if (anim != null) anim.speed = 0f;
    }
}