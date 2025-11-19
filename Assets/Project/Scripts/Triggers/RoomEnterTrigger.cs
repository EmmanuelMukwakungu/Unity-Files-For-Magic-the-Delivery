using UnityEngine;

public class RoomEnterTrigger : MonoBehaviour
{
    public RoomController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.PlayerEnteredRoom();
        }
    }
}
