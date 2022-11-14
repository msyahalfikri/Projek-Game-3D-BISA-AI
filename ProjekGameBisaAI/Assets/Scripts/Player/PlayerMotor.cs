using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static scr_model;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;

    private InputManager input;
    [SerializeField]
    public Vector3 playerVelocity;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    public bool crouching;
    public bool sprinting;
    public float crouchTimer = 0f;
    public bool lerpCrouch;

    private bool isGrounded;

    public LayerMask groundMask;

    public PlayerSettingsModel playerSettings;

    public bool isAimingIn;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = controller.isGrounded;
        crouchTimer += Time.deltaTime;
        float p = crouchTimer / 1;
        p *= p;
        if (crouching)
        {
            controller.height = Mathf.Lerp(controller.height, 1, p);
        }
        else
        {
            controller.height = Mathf.Lerp(controller.height, 2, p);
        }
        if (p > 1)
        {
            lerpCrouch = false;
            crouchTimer = 0f;
        }
        CalculateAimingIn();


    }
    //Menerima input dari inputmanager.cs
    public void processMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);


    }

    private void CalculateAimingIn()
    {
        if (!input.currentWeapon)
        {
            return;
        }

        input.currentWeapon.isAimingIn = isAimingIn;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0f;
        lerpCrouch = true;
    }
    public void Sprint()
    {
        if (!isAimingIn)
        {
            sprinting = !sprinting;

            if (sprinting)
            {
                speed = 8;
            }
            else
            {
                speed = 3.5f;
            }
        }

    }

    public void StopSprint()
    {
        sprinting = false;
        speed = 3.5f;
    }
    public void AimingInPressed()
    {
        isAimingIn = true;
    }
    public void AimingInReleased()
    {
        isAimingIn = false;
    }
}
