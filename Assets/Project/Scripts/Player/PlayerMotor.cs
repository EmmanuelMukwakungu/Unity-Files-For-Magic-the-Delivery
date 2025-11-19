using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour
{
    [Header("References")]
    private CharacterController _controller;
    private Animator _animator;
    private InputManager _input;
    private CameraMovement _cam;
    
    [Header("Speed Parameters")]
    private Vector3 playerVelocity;
   [SerializeField] public float speed = 5f;
   [SerializeField] public bool sprinting;
   
   [Header("Gravity Parameters")]
   [SerializeField] public float gravity = -9.81f;
   [SerializeField] public bool isGrounded;
   [SerializeField] public float jumpHeight = 3f;
   

   [Header("Weapon Handling")] 
   public WeaponController equippedWeapon;
   [SerializeField] public Transform weaponHoldPoint;

   

   public const string IDLEHoldMelee = "Idle";
  
   public const string WALK = "Walk";

   private string currentAnimationState;
   
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<InputManager>();
        _cam = GetComponent<CameraMovement>();
        _animator = GetComponentInChildren<Animator>();
        
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = _controller.isGrounded;
        
        //Repeat Inputs
        if (_input._playerMovement.MeeleAttack.triggered && equippedWeapon != null)
        {
            //Attack();
            equippedWeapon.MeleeAttack();
        }
        
        SetAnimations();
        
    }
    //Get input for InputManager.cs and apply them to character Controller
    public void ProcessMove(Vector3 input)
    {
        //Making the player move
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        
        //_controller.Move(transform.TransformDirection(moveDirection) * speed * Time.fixedDeltaTime);
        Vector3 move = transform.TransformDirection(moveDirection) * speed * Time.deltaTime; 
        _controller.Move(move);

        playerVelocity.x = moveDirection.x;
        playerVelocity.z = moveDirection.z;
        
        playerVelocity.y += gravity * Time.fixedDeltaTime;
        
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        _controller.Move(playerVelocity * Time.fixedDeltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
        {
            speed = 8;
            Debug.Log("We're sprinting");
        }
        else
        {
            speed = 5;
        }
    }

    #region Storing currently equipped weapons and unequipping them

    public void EquipWeapon( WeaponController newWeapon)
    {
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon.gameObject); //Remove old weapon
        }

        equippedWeapon = Instantiate(newWeapon, weaponHoldPoint);
        equippedWeapon.playerMotor = this;
        equippedWeapon.SetCamera(_cam);

        if (equippedWeapon.weaponData.type == WeaponType.Bullet)
        {
            ChanAnimationState("HoldGun");
        }
        else
        {
            ChanAnimationState(IDLEHoldMelee);
        }
    }

    public void UnequipWeapon()
    {
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon.gameObject);
        }
        
        equippedWeapon = null;
        ChanAnimationState("Player Idle");
    }

    #endregion

    #region Handling Melee Attack

    public void HandlePrimaryAttack()
    {
        if (equippedWeapon == null) return;
        {
            if (equippedWeapon.weaponData.type == WeaponType.Bullet)
            
                equippedWeapon.Shoot();
            
            else
            
                equippedWeapon.MeleeAttack();
            
        }
    }

    #endregion
    

   
    public void SetAnimations()
    {
        //If player is not Attacking
        if (equippedWeapon != null && equippedWeapon.IsAttacking())
        {
            if (Mathf.Abs(playerVelocity.x) < 0.1f && Mathf.Abs(playerVelocity.z) < 0.1f)
                ChanAnimationState(IDLEHoldMelee);
            else
                ChanAnimationState(WALK);
        }
    }

    public void ChanAnimationState(string newState)
    {
        if(currentAnimationState == newState) return;
        
        currentAnimationState = newState;
        _animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
    }
}
