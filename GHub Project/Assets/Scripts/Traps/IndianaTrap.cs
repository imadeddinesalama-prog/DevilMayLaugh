using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class IndianaTrap : MonoBehaviour
{
    [Header("Platforms — assign in order 1 2 3 4")]
    public SequentialFallingPlatform[] platforms;

    [Header("Delay between each platform fall")]
    public float delayBetweenFalls = 0.4f;

    private bool hasTriggered = false;

    void Start()
    {
        hasTriggered = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(TriggerSequence());
        }
    }

    IEnumerator TriggerSequence()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            if (platforms[i] != null)
            {
                platforms[i].TriggerFall();
            }

            yield return new WaitForSeconds(delayBetweenFalls);
        }
    }
}
