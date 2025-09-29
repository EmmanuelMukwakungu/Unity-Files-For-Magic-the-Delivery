using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 playerVelocity;
   [SerializeField] public float speed = 5f;
   [SerializeField] public bool sprinting;
   [SerializeField] public float gravity = -9.81f;
   [SerializeField] public bool isGrounded;
   [SerializeField] public float jumpHeight = 3f;
   [SerializeField] public Transform weaponHoldPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = _controller.isGrounded;
    }
    //Get input for InputManager.cs and apply them to character Controller
    public void ProcessMove(Vector3 input)
    {
        //Making the player move
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        
        _controller.Move(transform.TransformDirection(moveDirection) * speed * Time.fixedDeltaTime);
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
}
