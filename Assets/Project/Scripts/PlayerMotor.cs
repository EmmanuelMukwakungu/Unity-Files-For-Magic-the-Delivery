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
   [SerializeField] public Transform weaponHoldPoint;

   [Header("Attack Parameters")] 
   public float attackDistance = 3f;
   public float attackDelay = 0.4f;
   public float attackSpeed = 1f;
   public int attackDamage = 1;
   public LayerMask attackLayer;
   
   public GameObject hitEffect;
   public AudioClip swordSwing;
   public AudioClip hitSound;
   
   bool attacking = false;
   private bool readyToAttack = true;
   private int attackCount;

   public const string IDLE = "Idle";
   public const string ATTACK1 = "Attack 1";
   public const string ATTACK2 = "Attack 2";
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
        if (_input._playerMovement.MeeleAttack.IsPressed())
        {
            Attack();
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

    public void Attack()
    {
        if(!readyToAttack || attacking) return;
        readyToAttack = false;
        attacking = true;
        
        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);
        
        //audioSource.pitch = Random.Range(0.9f, 1.1f);
        //audioSource.PlayOneShot(swordSwing);

        if (attackCount == 0)
        {
            ChanAnimationState(ATTACK1);
            attackCount++;
        }
        else
        {
            ChanAnimationState(ATTACK2);
            attackCount = 0;
        }
    }

    public void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    public void AttackRaycast()
    {
        if (Physics.Raycast(_cam._camera.transform.position, _cam._camera.transform.forward, out RaycastHit hit,
                attackDistance, attackLayer))
        {
            HitTarget(hit.point);
            if(hit.transform.TryGetComponent<EnemyHealth>(out EnemyHealth H))
            {
                H.TakeDamage(attackDamage);
            }
        }

    }

    public void HitTarget(Vector3 pos)
    {
        //audioSource.pitch = 1;
        //audioSource.PlayOneShot(hitSound);
        
        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(GO, 20);
    }

    public void SetAnimations()
    {
        //If player is not Attacking
        if (!attacking)
        {
            if (playerVelocity.x == 0 && playerVelocity.z == 0)
            {
                ChanAnimationState(IDLE);
            }
            else
            {
                ChanAnimationState(WALK);
            }
        }
    }

    public void ChanAnimationState(string newState)
    {
        if(currentAnimationState == newState) return;
        
        currentAnimationState = newState;
        _animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
    }
}
