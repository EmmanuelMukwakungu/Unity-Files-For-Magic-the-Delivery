using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponController : MonoBehaviour
{
    AudioSource audioSource; 
    Animator weaponAnimator;

    public Transform muzzle;
    public WeaponData weaponData;

    public PlayerMotor playerMotor { get; set; }
    public int ammoCount { get; private set; }
    
    private bool reloading = false;
    private bool shooting = false; 
    private bool readyToShoot = true;
    
    public const string SHOOT = "Shoot";
    public const string RELOAD = "Reload";

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        weaponAnimator = GetComponentInChildren<Animator>();
        
        ammoCount = weaponData.maxAmmo;
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
