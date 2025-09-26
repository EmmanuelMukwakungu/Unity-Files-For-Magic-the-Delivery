using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    PlayerActions _playerActions;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 12f;
    [SerializeField] private float lookSensitivity = 2f;
    [SerializeField] private Transform cameraTransform;
    
    Rigidbody playerRigidbody;
    Vector3 movementVector; 
    Vector2 cameraVector;
    float xRotation = 0f;
    
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] float groundCheckRadius;
    [SerializeField] bool isGrounded;

    private void Awake()
    {
        _playerActions = new PlayerActions();
        _playerActions.Enable();
        
        playerRigidbody = GetComponent<Rigidbody>();
        _playerActions.PlayerMovement.Movement.performed += context => OnMove(context);
        _playerActions.PlayerMovement.Movement.canceled += context => OnStop(context);
        _playerActions.PlayerMovement.Camera.performed += context => OnLook(context);
        _playerActions.PlayerMovement.Camera.canceled += context => cameraVector = Vector2.zero;

        /**Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
    }

    private void FixedUpdate()
    {
        Vector3 move = transform.right * movementVector.x + transform.forward * movementVector.z;
        playerRigidbody.MovePosition(transform.position + move * moveSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        float cameraInputX = cameraVector.x * lookSensitivity;
        float cameraInputY = cameraVector.y * lookSensitivity;
        
        transform.Rotate(Vector3.up * cameraInputX);
        
        xRotation -= cameraInputY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        movementVector = new Vector3(input.x, 0, input.y);
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        movementVector = Vector3.zero;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        cameraVector = context.ReadValue<Vector2>();
    }
}
