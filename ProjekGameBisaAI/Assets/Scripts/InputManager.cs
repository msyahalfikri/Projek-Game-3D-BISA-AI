using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnfootActions onFoot;
    private PlayerMotor motor;
    private PlayerLook look;
    private bool isJumpHeld;
    private CharacterController controller;



    [Header("Weapon")]
    public WeaponController currentWeapon;
    public float weaponAnimationSpeed;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.Onfoot;
        motor = GetComponent<PlayerMotor>();
        onFoot.Jump.performed += ctx => motor.Jump();
        look = GetComponent<PlayerLook>();
        controller = GetComponent<CharacterController>();

        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.performed += ctx => motor.Sprint();

        onFoot.Movement.started += ctx => WeaponMovementSway(true);
        onFoot.Movement.performed += ctx => WeaponMovementSway(true);
        onFoot.Movement.canceled += ctx => WeaponMovementSway(false);

        onFoot.SprintReleased.performed += ctx => motor.StopSprint();

        if (currentWeapon)
        {
            currentWeapon.initialize(this);
        }
    }
    public void WeaponMovementSway(bool state)
    {
        if (state)
        {
            weaponAnimationSpeed = 1;
        }
        else
        {
            weaponAnimationSpeed = 0;
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Menyuruh playermotor untuk bergerak menggunakan nilai dari movement action
        motor.processMove(onFoot.Movement.ReadValue<Vector2>());
    }
    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
