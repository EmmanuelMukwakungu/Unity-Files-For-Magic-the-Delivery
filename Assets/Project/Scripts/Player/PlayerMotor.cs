using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    private Animator animator;
    private InputManager input;
    public CameraMovement cam;
    private WeaponController equippedWeapon;
    
    [Header("Speed Parameters")]
    private Vector3 playerVelocity;
    [SerializeField] public float speed = 5f;
    [SerializeField] public bool sprinting;
   
    [Header("Gravity Parameters")]
    [SerializeField] public float gravity = -9.81f;
    [SerializeField] public bool isGrounded;
    [SerializeField] public float jumpHeight = 3f;
   

    [Header("Weapon Handling")] 
    //public WeaponData defaultWeapon;
    [SerializeField] public Transform weaponHoldPoint;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<InputManager>();
        cam = GetComponent<CameraMovement>();
        animator = GetComponentInChildren<Animator>();
        
       // SetWeapon(defaultWeapon);
        
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (input.shootActions.Shoot.IsPressed() && equippedWeapon.weaponData.automatic)
        {
            equippedWeapon.Shoot();
        }
    }
    
    public void ProcessMove(Vector3 input)
    {
        //Making the player move
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        
        //_controller.Move(transform.TransformDirection(moveDirection) * speed * Time.fixedDeltaTime);
        Vector3 move = transform.TransformDirection(moveDirection) * speed * Time.deltaTime; 
        controller.Move(move);

        playerVelocity.x = moveDirection.x;
        playerVelocity.z = moveDirection.z;
        
        playerVelocity.y += gravity * Time.fixedDeltaTime;
        
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        controller.Move(playerVelocity * Time.fixedDeltaTime);
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

    public void SetWeapon(WeaponData newWeapon)
    {
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon.gameObject);
        }

        equippedWeapon = Instantiate(newWeapon.weaponPrefab, weaponHoldPoint);
        equippedWeapon.myMotor = this;
    }

    public void PlayAnimation(string newState)
    {
        animator.Play(newState);
    }

}
