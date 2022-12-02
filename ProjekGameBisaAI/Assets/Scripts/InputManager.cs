using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnfootActions onFoot;
    public PlayerInput.WeaponActions weaponActions;

    public PlayerInput.MenuActions menu;
    private PlayerMotor motor;
    private PlayerLook look;
    private bool isJumpHeld;
    private CharacterController controller;
    public GameObject[] weaponHolder;

    public GameObject pauseObj;
    public PauseMenu pauseScr;

    [Header("Weapon")]
    public WeaponController currentWeapon;
    public float weaponAnimationSpeed;

    public int weaponIndex = 0;

    void Awake()
    {
        WeaponMovementSway(false);

        playerInput = new PlayerInput();
        onFoot = playerInput.Onfoot;
        weaponActions = playerInput.Weapon;
        menu = playerInput.Menu;

        motor = GetComponent<PlayerMotor>();

        look = GetComponent<PlayerLook>();

        pauseScr = pauseObj.GetComponent<PauseMenu>();

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

        weaponActions.SwitchWeaponPrevious.performed += ctx => SwitchingPressed(0);
        weaponActions.SwitchWeaponNext.performed += ctx => SwitchingPressed(1);
        onFoot.Jump.performed += ctx => motor.Jump();
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

    }
    private void SwitchingPressed(int index)
    {
        currentWeapon.SwitchWeapon(index);
    }

    private void Update()
    {
        if (currentWeapon)
        {
            currentWeapon.initialize(this);
        }

    }
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
        menu.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
        weaponActions.Disable();
        menu.Disable();
    }
}
