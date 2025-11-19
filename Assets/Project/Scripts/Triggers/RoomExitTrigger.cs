using UnityEngine;

public class RoomExitTrigger : MonoBehaviour
{
    public RoomController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.OnEnemyDied();
        }
    }
}
