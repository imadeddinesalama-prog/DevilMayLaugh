using UnityEngine;

public class GravityFlipObject : MonoBehaviour
{
    public GravityZone gravityZone;   // drag the zone here

    private bool hasBeenShot = false;

    public void TakeHit()
    {
        if (hasBeenShot) return;
        hasBeenShot = true;

        if (gravityZone != null)
            gravityZone.ActivateFlip();

        Destroy(gameObject, 0.2f);
    }
}