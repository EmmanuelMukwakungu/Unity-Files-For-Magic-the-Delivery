using TMPro;
using UnityEngine;

public class GunWeapon : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bullet;

    [Header("Bullet Force")]
    public float shootForce, upwardForce;

    [Header("Gun Stats")]
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public int totalAmmo = 90; // NEW: total reserve ammo
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    [Header("Weapon Recoil")]
    public CharacterController playerController;
    public float recoilBackForce = 0.2f;
    public float recoilUpForce = 0.1f;
    public Transform Camera;

    [Header("Bools")]
    bool shooting, readyToShoot, reloading;

    [Header("References")]
    public Camera fpsCam;
    public Transform attackPoint;
    private InputManager input;

    [Header("Graphics")]
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    public bool allowInvoke = true;

    private void Awake()
    {
        input = FindObjectOfType<InputManager>();
        bulletsLeft = magazineSize;
        readyToShoot = true;
        
        if (ammunitionDisplay != null)
            ammunitionDisplay.gameObject.SetActive(false);
    }

    private void Update()
    {
        MyInput();

        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft + " / " + totalAmmo); // UPDATED: show total ammo instead of magazineSize
    }

    private void MyInput()
    {
        if (allowButtonHold)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = input.shootActions.Shoot.IsPressed();

        if (input.shootActions.Reload.IsPressed() && bulletsLeft < magazineSize && !reloading)
            Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
            Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        // Find hit position
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint = Physics.Raycast(ray, out hit) ? hit.point : ray.GetPoint(75);

        // Direction
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        // Add spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        // Instantiate bullet
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;
        
        // 💥 Auto-destroy bullet after 5 seconds
        Destroy(currentBullet, 5f);
        
        Rigidbody bulletRb = currentBullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        bulletRb.AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke(nameof(ResetShot), timeBetweenShooting);
            allowInvoke = false;
            ApplyRecoil(directionWithSpread);
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke(nameof(Shoot), timeBetweenShots);
    }

    private void ApplyRecoil(Vector3 direction)
    {
        Vector3 recoilDir = -fpsCam.transform.forward * recoilBackForce;
        playerController.Move(recoilDir * Time.deltaTime * 60f);

        if (Camera != null)
            Camera.localRotation *= Quaternion.Euler(-recoilUpForce, 0f, 0f);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        if (totalAmmo <= 0 || bulletsLeft == magazineSize) return; // NEW: no ammo left or already full

        reloading = true;
        Invoke(nameof(ReloadFinished), reloadTime);
    }

    private void ReloadFinished()
    {
        int bulletsNeeded = magazineSize - bulletsLeft; // NEW: how many bullets to refill
        int bulletsToLoad = Mathf.Min(bulletsNeeded, totalAmmo); // NEW: take only what’s available

        bulletsLeft += bulletsToLoad; // fill up
        totalAmmo -= bulletsToLoad;   // reduce reserve

        reloading = false;
    }
    
    public void SetAmmunitionDisplayVisible(bool visible)
    {
        if (ammunitionDisplay != null)
            ammunitionDisplay.gameObject.SetActive(visible);
    }

}
