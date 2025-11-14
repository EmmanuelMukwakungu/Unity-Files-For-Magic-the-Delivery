using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
  private PlayerActions _playerActions;
  public PlayerActions.PlayerMovementActions _playerMovement;
  private PlayerMotor _motor;
  private CameraMovement _cameraMovement;

  void Awake()
  {
    _playerActions = new PlayerActions();
    _playerMovement = _playerActions.PlayerMovement;
    
    _motor = GetComponent<PlayerMotor>();
    
    _cameraMovement = GetComponent<CameraMovement>();
    
    _playerMovement.Jump.performed += context => _motor.Jump();
    _playerMovement.Sprint.performed += context => _motor.Sprint();
    _playerMovement.MeeleAttack.performed += context => _motor.HandlePrimaryAttack();
    _playerMovement.Shoot.performed += context => _motor.equippedWeapon.Shoot();
    _playerMovement.Reload.performed += context => _motor.equippedWeapon.Reload();
  }

  void FixedUpdate()
  {
    _motor.ProcessMove(_playerMovement.Movement.ReadValue<Vector2>());
  }

  private void LateUpdate()
  {
    _cameraMovement.ProcessLook(_playerMovement.Camera.ReadValue<Vector2>());
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
