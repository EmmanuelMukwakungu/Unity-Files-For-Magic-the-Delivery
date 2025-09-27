using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
  private PlayerActions _playerActions;
  private PlayerActions.PlayerMovementActions _playerMovement;
  private PlayerMotor _motor;

  void Awake()
  {
    _playerActions = new PlayerActions();
    _playerMovement = _playerActions.PlayerMovement;
    _motor = GetComponent<PlayerMotor>();
    
    _playerMovement.Jump.performed += context => _motor.Jump();
  }

  void FixedUpdate()
  {
    _motor.ProcessMove(_playerMovement.Movement.ReadValue<Vector2>());
  }

  private void OnEnable()
  {
    _playerMovement.Enable();
  }

  private void OnDisable()
  {
    _playerMovement.Disable();
  }
}
