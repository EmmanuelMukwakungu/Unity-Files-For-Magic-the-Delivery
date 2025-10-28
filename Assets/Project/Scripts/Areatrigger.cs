using System;
using UnityEngine;

public class Areatrigger : MonoBehaviour
{
    public GameObject TriggerArea;
    public EnemyHealth enemyHealth;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            enemyHealth.TakeDamage(1);
            Debug.Log("Enemy took damage!");
        }
    }
}
