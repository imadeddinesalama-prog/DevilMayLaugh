using UnityEngine;

public class CrusherManager : MonoBehaviour
{
    [Header("All 6 crusher platforms")]
    public CrusherPlatform[] platforms;

    [Header("Stagger — delay between each pair starting")]
    public float staggerDelay = 0f;  

    void Start()
    {
        StartCoroutine(LaunchAll());
    }

    System.Collections.IEnumerator LaunchAll()
    {
        foreach (CrusherPlatform p in platforms)
        {
            if (p != null)
            {
                p.Initialize();
                p.StartCrushing();
                if (staggerDelay > 0f)
                    yield return new WaitForSeconds(staggerDelay);
            }
        }
    }
}   