using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    MenuManager menuManager;
    private float health;
    public float maxHealth = 100;
    private float lerpTimer;
    public float chipSpeed = 2f;
    
    public Image FrontHealthBar;
    public Image BackHealthBar;

    public Image damageOverlay;
    public GameObject GameOverScreen;
    public float duration;
    public float fadeSpeed;

    private float durationTimer;
    
    void Start()
    {
        health = maxHealth;
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 0);

    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        if (damageOverlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                //fade image
                float tempAlpha = damageOverlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, tempAlpha);
            }
        }
    }

    public void UpdateHealthUI()
    {
        //Debug.Log(health);
        float fillFront = FrontHealthBar.fillAmount;
        float fillBack = BackHealthBar.fillAmount;
        float HealthFraction = health / maxHealth;
        
        if (fillBack > HealthFraction)
        {
            FrontHealthBar.fillAmount = HealthFraction;
            BackHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentage = lerpTimer / chipSpeed;
            BackHealthBar.fillAmount = Mathf.Lerp(fillBack, HealthFraction, percentage);
        }
        else if (fillFront < HealthFraction)
        {
            BackHealthBar.fillAmount = HealthFraction;
            BackHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentage = lerpTimer / chipSpeed;
            FrontHealthBar.fillAmount = Mathf.Lerp(fillFront, HealthFraction, percentage);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0f;
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, 1);
        
        if (health == 0)
        {
            GameOverScreen.SetActive(true);
            menuManager.crossHair.enabled = false;
            menuManager.pauseBtn.enabled = false;
        }


    }

    public void RestoreHealth(float healAmount)
    {
        if (health < maxHealth)
        {
            health += healAmount;
            health = Mathf.Clamp(health, 0, maxHealth);
            lerpTimer = 0f;
        }

    }
}
