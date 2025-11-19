using System;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public event System.Action OnWeaponPickedUp;
    
    public GunWeapon weaponScript;
    public SwordWeapon swordWeapon; //assign if it's a sword
    private InputManager input;

    public BoxCollider boxCollider;
    public Transform playerTransform;
    public Transform weaponHoldPoint;
    public Transform fpsCam;

    public float pickUpRange = 2f;
    public float dropDistance = 2f; // distance in front of player when dropped

    public bool equipped;
    public static bool slotFull;

    public Vector3 rotationSpeed = new Vector3(0f, 50f, 0f);
    public float hoverAmplitude = 0.1f;  // How far it moves up/down
    public float hoverFrequency = 2f;
    private float startY;

    private void Start()
    {
        input = FindObjectOfType<InputManager>();
        startY = transform.position.y;

        //Disable both weapon scripts initially if not equipped
        if (!equipped)
        {
           if (weaponScript !=null) weaponScript.enabled = false;
           if (swordWeapon != null) swordWeapon.enabled = false;
           
            boxCollider.isTrigger = false;
        }
        else
        {
            if (weaponScript !=null) weaponScript.enabled = true;
            if (swordWeapon != null) swordWeapon.enabled = true;
            
            boxCollider.isTrigger = true;
            slotFull = true;
        }
    }

    private void Update()
    {
        if (!equipped)
        {
            HoverAndRotate();
        }

        Vector3 distanceToPlayer = playerTransform.position - transform.position;

        // Pick up weapon
        if (!equipped && distanceToPlayer.magnitude < pickUpRange &&
            input.playerMovement.Interact.IsPressed() && !slotFull)
        {
            PickUp();
        }

        // Drop weapon
        if (equipped && Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        transform.SetParent(weaponHoldPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        boxCollider.isTrigger = true;
        
        //Enable the correct weapon type
        if (weaponScript != null)
        {
            weaponScript.enabled = true;
            weaponScript.SetAmmunitionDisplayVisible(true);
        }
        
        SwordWeapon sword = GetComponent<SwordWeapon>();
        if (sword != null)
        {
            sword.enabled = true;
            sword.SetEquipped(true);
        } 
        
        OnWeaponPickedUp?.Invoke();
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        transform.SetParent(null);

        // place weapon slightly in front of the player
        transform.position = fpsCam.position + fpsCam.forward * dropDistance;
        transform.rotation = Quaternion.identity;

        boxCollider.isTrigger = false;
        if (weaponScript != null)
        {
            weaponScript.enabled = false;
            weaponScript.SetAmmunitionDisplayVisible(false);
        }
        SwordWeapon sword = GetComponent<SwordWeapon>();
        if (sword != null)
        {
            sword.enabled = false;
            sword.SetEquipped(false);
        } 
    }
    
    private void HoverAndRotate()
    {
        // Rotate smoothly
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.World);
        
        // Hover up and down only (don’t override X/Z)
        float newY = startY + Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
