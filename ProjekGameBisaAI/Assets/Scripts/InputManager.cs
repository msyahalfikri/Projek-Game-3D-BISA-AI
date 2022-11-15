using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnfootActions onFoot;
    public PlayerInput.WeaponActions weaponActions;
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
        weaponActions = playerInput.Weapon;

        motor = GetComponent<PlayerMotor>();

        onFoot.Jump.performed += ctx => motor.Jump();

        look = GetComponent<PlayerLook>();

        controller = GetComponent<CharacterController>();

        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.performed += ctx => motor.Sprint();
        onFoot.SprintReleased.performed += ctx => motor.StopSprint();

        onFoot.Movement.started += ctx => WeaponMovementSway(true);
        onFoot.Movement.performed += ctx => WeaponMovementSway(true);
        onFoot.Movement.canceled += ctx => WeaponMovementSway(false);

        weaponActions.Fire2Pressed.performed += ctx => motor.AimingInPressed();
        weaponActions.Fire2Released.performed += ctx => motor.AimingInReleased();

        weaponActions.Fire1Pressed.performed += ctx => ShootingPressed();
        weaponActions.Fire1Released.performed += ctx => ShootingReleased();
        weaponActions.Reload.performed += ctx => StartCoroutine(currentWeapon.ReloadWeapon());


        if (currentWeapon)
        {
            currentWeapon.initialize(this);
        }
    }
    private void ShootingPressed()
    {
        if (currentWeapon)
        {
            currentWeapon.isShooting = true;
        }
    }
    private void ShootingReleased()
    {
        if (currentWeapon)
        {
            currentWeapon.isShooting = false;
        }
    }
    public void WeaponMovementSway(bool state)
    {
        if (state)
        {
            currentWeapon.weaponAnimator.SetBool("isWalking", state);
            currentWeapon.weaponAnimator.SetBool("isIdle", false);

        }
        else
        {
            currentWeapon.weaponAnimator.SetBool("isWalking", state);
            currentWeapon.weaponAnimator.SetBool("isIdle", true);
        }

        // if (state)
        // {
        //     if (currentWeapon.isAimingIn)
        //     {
        //         weaponAnimationSpeed = 0.3f;
        //     }
        //     else
        //     {
        //         weaponAnimationSpeed = 1;
        //     }

        // }
        // else
        // {
        //     weaponAnimationSpeed = 0;
        // }

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
        weaponActions.Enable();

    }

    private void OnDisable()
    {
        onFoot.Disable();
        weaponActions.Disable();
    }
}
