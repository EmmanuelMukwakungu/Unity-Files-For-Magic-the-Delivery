using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //Shooting and Reloading Animations
   public const string SHOOT = "Shoot";
   public const string RELOAD = "Reload";

   private AudioSource audioSource;
   Animator weaponAnimator;
   
   public Transform muzzle;
   public WeaponData weaponData;

   public PlayerMotor myMotor {get; set; }
   public int ammoCount { get; private set; }
   
   bool reloading = false;
   bool shooting = false; 
   bool readyToShoot = true;

   void Awake()
   {
      audioSource = GetComponent<AudioSource>();
      weaponAnimator = GetComponentInChildren<Animator>();
      
       ammoCount = weaponData.maxAmmo;
       UserInterface.singleton.UpdateBulletCounter(ammoCount, weaponData.maxAmmo);
   }
   
   //Shooting Behaviour
   public void Shoot()
   {
       if(!readyToShoot || shooting || reloading || weaponData == null ) return;
       if (ammoCount <= 0)
       {
           Reload(); return;
       }

       readyToShoot = false;
       shooting = true;

       UseAmmo();
       
       Invoke(nameof(ResetShootingAttack), weaponData.fireRate);
       AttackRaycast();

       Instantiate(weaponData.fireEffect, muzzle);
       
       audioSource.pitch = Random.Range(0.9f, 1.1f);
       audioSource.PlayOneShot(weaponData.fireSound);
       
       weaponAnimator.Play(SHOOT);
       myMotor.PlayAnimation(SHOOT);
   }

   public void UseAmmo()
   {
       ammoCount--;
       UserInterface.singleton.UpdateBulletCounter(ammoCount, weaponData.maxAmmo);
   }

   public void Reload()
   {
       if(ammoCount == weaponData.maxAmmo || reloading) return;
       reloading = true;
       
       myMotor.PlayAnimation(RELOAD);
       weaponAnimator.Play(RELOAD);
       Invoke(nameof(ResetReload), weaponData.reloadTime);
   }

   public void ResetShootingAttack()
   {
       shooting = false;
       readyToShoot = true;
   }

  public void ResetReload()
   {
       reloading = false;
       ammoCount = weaponData.maxAmmo;
       UserInterface.singleton.UpdateBulletCounter(ammoCount, weaponData.maxAmmo);
   }

   public void AttackRaycast()
   {
       RaycastHit hit;
       RaycastHit[] hits;
       
       switch (weaponData.type)
       {
           case WeaponType.Bullet:
           if (Physics.Raycast(myMotor.transform.position, myMotor.transform.forward, out hit, weaponData.weaponRange))
           {
               HitTarget(hit);
           }
           break; 
           
           case WeaponType.Piercing:
               hits = Physics.RaycastAll(myMotor.cam.transform.position, myMotor.cam.transform.forward,
                   weaponData.weaponRange);

               if (hits.Length > 0)
               {
                   for (int i = 0; i < hits.Length; i++)
                   {
                       HitTarget(hits[i]);
                   }
               }
               break;
       }
           
           
   }

 public void HitTarget(RaycastHit hit)
   {
       
       GameObject GO = Instantiate(weaponData.hitEffect, hit.point, Quaternion.identity);
       Destroy(GO, 4);
   }

}
