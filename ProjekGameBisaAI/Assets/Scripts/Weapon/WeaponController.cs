using static scr_model;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private InputManager input;

    private PlayerMotor motor;

    [Header("References")]
    public Animator weaponAnimator;
    [Header("Settings")]
    public WeaponSettingsModel settings;

    bool isInitialized;

    Vector3 newWeaponRotation;
    Vector3 newWeaponRotationVelocity;

    Vector3 targetWeaponRotation;
    Vector3 targetWeaponRotationVelocity;


    Vector3 newWeaponMovementRotation;
    Vector3 newWeaponMovementRotationVelocity;

    Vector3 targetWeaponMovementRotation;
    Vector3 targetWeaponMovementRotationVelocity;

    private void Start()
    {
        newWeaponRotation = transform.localRotation.eulerAngles;
        motor = GetComponentInParent<PlayerMotor>();
    }
    public void initialize(InputManager inputManager)
    {
        input = inputManager;
        isInitialized = true;
    }
    private void Update()
    {
        if (!isInitialized)
        {
            return;
        }
        CalculateWeaponRotation();
        SetWeaponAnimations();

    }

    private void CalculateWeaponRotation()
    {
        weaponAnimator.speed = input.weaponAnimationSpeed;

        Vector2 rotationValue = input.onFoot.Look.ReadValue<Vector2>();
        targetWeaponRotation.y += settings.swayAmount * (settings.swayXInverted ? -rotationValue.x : rotationValue.x) * Time.deltaTime;
        targetWeaponRotation.x += settings.swayAmount * (settings.swayYInverted ? rotationValue.y * 2 : -rotationValue.y * 2) * Time.deltaTime;

        targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -settings.swayClampX, settings.swayClampX);
        targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -settings.swayClampY, settings.swayClampY);
        targetWeaponRotation.z = targetWeaponRotation.y;

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, settings.swayResetSmoothing);
        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, settings.swaySmoothing);

        Vector2 movementValue = input.onFoot.Movement.ReadValue<Vector2>();
        targetWeaponMovementRotation.z = settings.movementSwayX * (settings.movementSwayXInverted ? -movementValue.x : movementValue.x);
        targetWeaponMovementRotation.x = settings.movementSwayY * (settings.movementSwayYInverted ? -movementValue.y : movementValue.y);

        targetWeaponMovementRotation = Vector3.SmoothDamp(targetWeaponMovementRotation, Vector3.zero, ref targetWeaponMovementRotationVelocity, settings.movementSwaySmoothing);
        newWeaponMovementRotation = Vector3.SmoothDamp(newWeaponMovementRotation, targetWeaponMovementRotation, ref newWeaponMovementRotationVelocity, settings.movementSwaySmoothing);

        transform.localRotation = Quaternion.Euler(newWeaponRotation + newWeaponMovementRotation);
    }

    private void SetWeaponAnimations()
    {
        weaponAnimator.SetBool("isSprinting", motor.sprinting);
    }
}
