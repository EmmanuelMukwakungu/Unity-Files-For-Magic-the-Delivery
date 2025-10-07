using System;
using UnityEngine;

public class Areatrigger : MonoBehaviour
{
    public GameObject TriggerArea;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            Debug.Log("trigger collided with player");
        }
    }
}
