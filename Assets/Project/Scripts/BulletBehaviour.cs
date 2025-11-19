using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        // Damage enemies
        EnemyHealth target = other.GetComponent<EnemyHealth>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        // Destroy the bullet after impact
        Destroy(gameObject);
    }
}
