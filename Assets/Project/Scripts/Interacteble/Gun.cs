using UnityEngine;

public class Gun : Interactable
{
    private bool isHeld = false;
    private Transform parent;
    private Collider col;
    void Start()
    {
        
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact(GameObject interactor)
    {
        PlayerMotor playerMotor = interactor.GetComponent<PlayerMotor>();
        if (playerMotor != null)
        {
            if (!isHeld)
            {
                parent = transform.parent;
                transform.SetParent(playerMotor.weaponHoldPoint);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler(Vector3.zero);
        
               
                col.enabled = false;
                isHeld = true;
            }
            else
            {
                transform.SetParent(null);
                col.enabled = true;
                isHeld = false;
            }

            
        }

    }
}
