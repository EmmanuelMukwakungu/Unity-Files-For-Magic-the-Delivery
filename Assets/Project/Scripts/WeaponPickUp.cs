using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    [SerializeField] private string weaponName = "Pistol";

    public void Interact(PlayerManager player)
    {
        Debug.Log($"Picked up {weaponName}");
        
        transform.SetParent(player.weaponHoldPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
}
