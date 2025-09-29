using UnityEngine;

public class Gun : Interactable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            transform.SetParent(playerMotor.weaponHoldPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }

    }
}
