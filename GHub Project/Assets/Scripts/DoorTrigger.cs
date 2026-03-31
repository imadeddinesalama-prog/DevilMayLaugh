using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public EscapeDoorMover door;
    public JumpBladeTrap blade;

    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            if (door != null)
                door.MoveDoor();

            if (blade != null)
                blade.TriggerTrap();
        }
    }
}