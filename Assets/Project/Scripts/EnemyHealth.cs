using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image healthBarSprite;
    
    [Header("Health Settings")]
    private float health;
    public float maxHealth = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        UpdateHealthBar();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
    }

    public void UpdateHealthBar()
    {
        if (healthBarSprite != null)
        {
            healthBarSprite.fillAmount = health / maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthBar();

        if (health <= 0)
        {
            Die();
        }

    }

    public void Die()
    {
        Debug.Log("Enemy has died");
        gameObject.SetActive(false);
    }
}
