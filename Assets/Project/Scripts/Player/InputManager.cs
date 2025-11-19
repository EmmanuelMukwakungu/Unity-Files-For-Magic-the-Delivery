using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Input References")] 
    private PlayerActions playerActions;
    
    public PlayerActions.PlayerMovementActions playerMovement;
    public PlayerActions.MeleeActions meleeActions;
    public PlayerActions.ShootWeaponActions shootActions;
    
    private PlayerMotor motor;
    private WeaponController WeaponMotor;
    private SwordWeapon sword;
    private CameraMovement cameraMovement;

    void Awake()
    {
        playerActions = new PlayerActions();
        playerMovement = playerActions.PlayerMovement;
        meleeActions = playerActions.Melee;
        shootActions = playerActions.ShootWeapon;
        
        motor = GetComponent<PlayerMotor>();
        cameraMovement = GetComponent<CameraMovement>();

        playerMovement.Jump.performed += context => motor.Jump();
        playerMovement.Sprint.performed += context => motor.Sprint();
        shootActions.Shoot.performed += context => WeaponMotor.Shoot();
        shootActions.Reload.performed += context => WeaponMotor.Reload();
        
    }
    
    void FixedUpdate()
    {
        motor.ProcessMove(playerMovement.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        cameraMovement.ProcessLook(playerMovement.Camera.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        playerMovement.Enable();
        //meleeActions.Enable();
        //shootActions.Enable();
    }

    private void OnDisable()
    {
        playerMovement.Disable();
        //meleeActions.Disable();
        //shootActions.Disable();
    }
}
