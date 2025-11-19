using System;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
   private void OnCollisionEnter(Collision other)
   {
      if (other.gameObject.CompareTag("Enemy"))
      {
         EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
         if (enemy != null)
         {
            enemy.TakeDamage(1);
            Debug.Log("Enemy Took -1 Damage!");
         }

         Destroy(gameObject); // destroy the bullet after impact
      }
   }
   
}
