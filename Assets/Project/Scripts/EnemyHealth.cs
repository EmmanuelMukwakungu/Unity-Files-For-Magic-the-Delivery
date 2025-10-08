using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float health;
    public float maxHealth = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        //lerpTimer = 0f;
        //durationTimer = 0f;
        //damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 1);

        
    }
}
