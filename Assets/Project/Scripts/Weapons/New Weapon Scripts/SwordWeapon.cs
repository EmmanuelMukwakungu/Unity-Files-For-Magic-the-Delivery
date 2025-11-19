
using System.Collections;
using UnityEngine;

public class SwordWeapon : MonoBehaviour
{
    private InputManager input;
    private Animator swordAnimator;
    
    private int currentSwing = 0;
    private bool isSwinging = false;
    
    public bool isEquipped = false;
    public float swingDuration = 0.5f;
    public float crossFadeDuration = 0.2f;
    public float damage = 1f;
    public float attackRange = 1.5f;

    public LayerMask enemyLayer;
    
    //public GameObject sword;
    
    void Start()
    {
        input = FindObjectOfType<InputManager>();
        swordAnimator = GetComponent<Animator>();
        
        if(!isEquipped && swordAnimator != null)
            swordAnimator.enabled = false;
        
    }

    void Update()
    {
        if (!isEquipped) return;
       SwordAttack();
    }

    public void SwordAttack()
    {
        if (input != null && Input.GetKeyDown(KeyCode.Mouse0) && !isSwinging)
        {
            Debug.Log("We're attacking");
            StartCoroutine(SwingSword());
        }
    }

    IEnumerator SwingSword()
    {
        isSwinging = true;
        
        string swingAnimation = currentSwing == 0 ? "SwordSwing1" : "SwordSwing2";
        currentSwing = 1 - currentSwing;
        
        swordAnimator.CrossFadeInFixedTime(swingAnimation, crossFadeDuration);

        // Wait a short moment before checking for hits (so the sword is in motion)
        yield return new WaitForSeconds(swingDuration * 0.3f);

        // Check for enemies in front of the sword
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward * attackRange * 0.5f, attackRange, enemyLayer);
        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log($"Hit {enemy.name} for {damage} damage!");
            }
        }

        // Finish animation
        yield return new WaitForSeconds(swingDuration);
        swordAnimator.CrossFadeInFixedTime("SwordIdle", crossFadeDuration);
        yield return new WaitForSeconds(0.2f);
        isSwinging = false;
    }

    public void SetEquipped(bool equipped)
    {
        isEquipped = equipped;
        if(swordAnimator != null)
            swordAnimator.enabled = equipped;
    } 
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRange * 0.5f, attackRange);
    }

}
