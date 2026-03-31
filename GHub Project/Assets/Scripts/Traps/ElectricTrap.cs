using UnityEngine;

public class ElectricTrap : MonoBehaviour
{
    [Header("References")]
    public GameObject warningLine;   
    public GameObject fullBeam;      

    [Header("Timing")]
    public float warningDuration = 1f;   
    public float beamDuration = 0.5f;    
    public float cooldown = 2f;          

    [Header("Beam Collider")]
    public Collider2D beamCollider;    

    private bool isBeamActive = false;

    void Start()
    {
        if (beamCollider == null && fullBeam != null)
            beamCollider = fullBeam.GetComponent<Collider2D>();

        warningLine.SetActive(false);
        fullBeam.SetActive(false);

        StartCoroutine(TrapCycle());
    }

    System.Collections.IEnumerator TrapCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown);

            warningLine.SetActive(true);
            fullBeam.SetActive(false);
            isBeamActive = false;

            yield return new WaitForSeconds(warningDuration);

            warningLine.SetActive(false);
            fullBeam.SetActive(true);
            isBeamActive = true;

            yield return new WaitForSeconds(beamDuration);

            fullBeam.SetActive(false);
            isBeamActive = false;
        }
    }
}