
using TMPro;
using UnityEngine;



public class PistolShoot : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    
    [Header("Bullet Force")]
    public float shootForce;
    public float upwardForce;
    
    [Header("Weapon Stats")]
    public float timeBetweenShots, spread, reloadTime, timeBetweenShooting;
    public int bulletsPerTap;
    public int magazineSize;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    bool shooting, readyToShoot, reloading;

    [Header("References")]
    public Camera mainCamera;
    public Transform attackPoint;
    public TextMeshProUGUI ammoText;
    public bool allowInvoke = true;
    public bool isEquiped = false;
    
    public Vector3 rotationSpeed = new Vector3(0f, 50f, 0f);
    

    private void Awake()
    {
        bulletsLeft = magazineSize; //Full Mag
        readyToShoot = true;
        ammoText.enabled = false;
        
    }

    private void Update()
    {
        if (ammoText != null)
        {
            ammoText.SetText(bulletsLeft / bulletsPerTap + "/" + magazineSize / bulletsPerTap);
        }
        
        if (transform.parent != null && transform.parent.CompareTag("Weapon Hold Point"))
        {
            //Can only shoot when weapon is equipped
            isEquiped = true;
            MyInputs();
            ammoText.enabled = true;
        }
        else
        {
            isEquiped = false;
            RotateObject();
        }
    }

    private void MyInputs()
    {
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
        
        Vector3 directionWithoutSpread = (targetPoint - attackPoint.position).normalized;
        
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        GameObject currentBullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);
        
        currentBullet.transform.forward = directionWithSpread.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        //currentBullet.GetComponent<Rigidbody>().AddForce(mainCamera.transform.up * upwardForce, ForceMode.Impulse);

        Destroy(currentBullet, 15f);
        
        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShots);
            allowInvoke = false;
        }
        
        //If more than one bullet per tap shoot function is repeated
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
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
    
    public void RotateObject()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
