using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera playerCamera;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask layerMask;
    private PlayerUI playerUI;
    private InputManager _inputManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera = GetComponent<CameraMovement>()._camera;
        playerUI = GetComponent<PlayerUI>();
        _inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        playerUI.UpdateText(String.Empty);
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
        RaycastHit hitInfo; // For storing collision info 
        if (Physics.Raycast(ray, out hitInfo, distance, layerMask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();  
                playerUI.UpdateText(interactable._promptMessage);
                if (_inputManager.playerMovement.Interact.triggered)//If E is pressed
                {
                    interactable.BaseInteract(gameObject);
                }
            }
        }
    }
}
