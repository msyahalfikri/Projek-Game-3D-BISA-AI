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
    public GameObject[] weaponHolder;

    public bool isSprinting;



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

        motor = GetComponent<PlayerMotor>();

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

        weaponActions.SwitchWeaponPrevious.performed += ctx => SwitchWeapon(0);
        weaponActions.SwitchWeaponNext.performed += ctx => SwitchWeapon(1);
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
    // Update is called once per frame
    private void Update()
    {
        isSprinting = motor.sprinting;
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

    public void SwitchWeapon(int index)
    {
        if (!currentWeapon.isReloading)
        {
            if (!currentWeapon.isAimingIn)
            {
                if (!currentWeapon.isShooting)
                {
                    if (!isSprinting)
                    {
                        currentWeapon = weaponHolder[index].GetComponent<WeaponController>();


                        if (index == 0)
                        {
                            weaponHolder[0].gameObject.SetActive(true);
                            weaponHolder[1].gameObject.SetActive(false);
                            weaponIndex = 0;
                        }
                        else if (index == 1)
                        {
                            weaponHolder[1].gameObject.SetActive(true);
                            weaponHolder[0].gameObject.SetActive(false);
                            weaponIndex = 1;
                        }
                        currentWeapon.updateWeaponUI();

                    }


                }
            }
        }





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
