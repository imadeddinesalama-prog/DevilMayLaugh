using UnityEngine;

public class SecretRoomEntrance : MonoBehaviour
{
    public SecretRoom secretRoom;

    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            secretRoom?.PlayerEntered();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            secretRoom?.PlayerExited();
    }
}