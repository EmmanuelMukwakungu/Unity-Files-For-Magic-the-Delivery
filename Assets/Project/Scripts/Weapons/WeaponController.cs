using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponController : MonoBehaviour
{
    AudioSource audioSource; 
    Animator weaponAnimator;
    private CameraMovement cam;

    public Transform muzzle;
    public WeaponData weaponData;

    public PlayerMotor playerMotor { get; set; }
    public int ammoCount { get; private set; }
    
    private bool reloading = false;
    private bool shooting = false; 
    private bool readyToShoot = true;

    #region Attacking Parameters

    [Header("Attack Parameters")] 
    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 1;
    public LayerMask attackLayer;
   
    public GameObject hitEffect;
    public AudioClip swordSwing;
    public AudioClip hitSound;
   
    bool attacking = false;
    private bool readyToAttack = true;
    private int attackCount;
    
    public const string ATTACK1 = "Attack 1";
    public const string ATTACK2 = "Attack 2";

    #endregion
    
    public const string SHOOT = "Shoot";
    public const string RELOAD = "Reload";

    private string currentAnimState;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        weaponAnimator = GetComponentInChildren<Animator>();
        cam = GetComponent<CameraMovement>();
        
        ammoCount = weaponData.maxAmmo;
    }

    #region Melee Attack Logic

    public void MeleeAttack()
    {
        if(!readyToAttack || attacking) return;
        readyToAttack = false;
        attacking = true;
        
        Invoke(nameof(ResetMelleAttack), attackSpeed);
        Invoke(nameof(MeleeAttackRaycast), attackDelay);
        
        //audioSource.pitch = Random.Range(0.9f, 1.1f);
        //audioSource.PlayOneShot(swordSwing);

        if (attackCount == 0)
        {
            ChanAnimationState(ATTACK1);
            attackCount++;
        }
        else
        {
            ChanAnimationState(ATTACK2);
            attackCount = 0;
        }
    }

    public void ResetMelleAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    public void MeleeAttackRaycast()
    {
        if (Physics.Raycast(cam._camera.transform.position, cam._camera.transform.forward, out RaycastHit hit,
                attackDistance, attackLayer))
        {
            HitTarget(hit.point);
            if(hit.transform.TryGetComponent<EnemyHealth>(out EnemyHealth H))
            {
                H.TakeDamage(attackDamage);
            }
        }

    }

    public void HitTarget(Vector3 pos)
    {
        //audioSource.pitch = 1;
        //audioSource.PlayOneShot(hitSound);
        
        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(GO, 20);
    }

    private void ChanAnimationState(string state)
    {
        if(currentAnimState == state) return;
        currentAnimState = state;
        weaponAnimator.CrossFadeInFixedTime(currentAnimState, 0.1f);
    }

    public bool IsAttacking()
    {
        return attacking;
    }

    #endregion
  
    //setting Camera
    public void SetCamera(CameraMovement playerCam)
    {
        cam = playerCam;
    }

    public void Shoot()
    {
        if(!readyToShoot || shooting || reloading || weaponData == null) return;
        if (ammoCount <= 0)
        {
            Reload(); return;
        }
        readyToShoot = false;
        shooting = true;

        UseAmmo();
        
        Invoke(nameof(ResetAttack), weaponData.fireRate);
        AttackRaycast();

        Instantiate(weaponData.fireEffect, muzzle);
        
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(weaponData.fireSound);
        
        weaponAnimator.Play(SHOOT);
        //playerMotor.PlayAnimation(SHOOT);
    }

    void UseAmmo()
    {
        ammoCount--;
    }

    public void Reload()
    {
        if (ammoCount == weaponData.maxAmmo || reloading) return;
        reloading = true;
        //playerMotor.PlayAnimation(RELOAD);
        weaponAnimator.Play(RELOAD);
        Invoke(nameof(ResetReload), weaponData.reloadTime);
    }

    void ResetAttack()
    {
        shooting = false;
        readyToShoot = true;
    }

    void ResetReload()
    {
        reloading = false;
        ammoCount = weaponData.maxAmmo;
    }

    void AttackRaycast()
    {
        RaycastHit hit;
        RaycastHit[] hits;

        /*switch (weaponData.type)
        {
            case WeaponType.Bullet:
                if (Physics.Raycast(playerMotor._cam.transform.position, playerMotor._cam.transform.forward, out hit,
                        weaponData.weaponRange))
                {
                    HitTargert(hit);
                }
                break;
            case WeaponType.Piercing:
                hits = Physics.RaycastAll(playerMotor._cam.transform.position, playerMotor._cam.transform.forward,
                    weaponData.weaponRange);
                if (hits.Length > 0)
                {
                    for (int i = 0; i < hits.Length; ++i)
                    {
                        HitTargert(hits[i]);
                    }
                }
                break;
        }*/
    }

    void HitTargert(RaycastHit hit)
    {
        /*if (hit.transform.parent.TryGetComponent<EnemyHealth>(out EnemyHealth H))
        {
            if (hit.transform.tag == "Head")
            {
                int dmg = Mathf.RoundToInt(weaponData.attackDamage * weaponData.headshotMultiplier);
                H.TakeDamage(dmg);
            }
            else
            {
                H.TakeDamage(weaponData.attackDamage);
            }
        }


        GameObject GO = Instantiate(weaponData.hitEffect, hit.point, quaternion.identity);
        Destroy(GO, 4);*/
    }
}
