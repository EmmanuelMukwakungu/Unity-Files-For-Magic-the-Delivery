using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image healthBarSprite;
    public RoomController roomController;

    [Header("Health Settings")]
    private float health;
    public float maxHealth = 3f;

    public event System.Action OnEnemyDeath;

    void Start()
    {
        health = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) 
        { TakeDamage(1); }
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

        if (roomController != null)
            roomController.OnEnemyDied();
        else
            Debug.LogError("RoomController is NOT assigned on Enemy!");

        gameObject.SetActive(false);
    }

    public void SetRoomController(RoomController controller)
    {
        roomController = controller;
    }
}

