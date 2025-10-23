
using TMPro;
using UnityEngine;



public class PistolShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    public float shootForce;
    public float upwardForce;
    public float timeBetweenShots, spread, reloadTime, timeBetweenShooting;

    public int bulletsPerTap;
    public int magazineSize;

    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;
    bool shooting, readyToShoot, reloading;

    public Camera mainCamera;
    public Transform attackPoint;
    
    public TextMeshProUGUI ammoText;

    public bool allowInvoke = true;
    public bool isEquiped = false;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInputs();

        if (ammoText != null)
        {
            ammoText.SetText(bulletsLeft / bulletsPerTap + "/" + magazineSize / bulletsPerTap);
        }
    }

    private void MyInputs()
    {
       // if(!isEquiped) return;
        
        if (allowInvoke)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            Reload();   
        }

        if (shooting && readyToShoot && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }

    }

    private void Shoot()
    {
        readyToShoot = false;
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }
        
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
        
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        GameObject currentBullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);
        
        currentBullet.transform.forward = directionWithSpread.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(mainCamera.transform.up * upwardForce, ForceMode.Impulse);

        Destroy(currentBullet, 15f);
        
        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShots);
            allowInvoke = false;
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
