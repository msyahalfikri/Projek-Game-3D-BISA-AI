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

    private PlayerLook look;

    bool isInitialized;

    Vector3 newWeaponRotation;
    Vector3 newWeaponRotationVelocity;

    Vector3 targetWeaponRotation;
    Vector3 targetWeaponRotationVelocity;


    Vector3 newWeaponMovementRotation;
    Vector3 newWeaponMovementRotationVelocity;

    Vector3 targetWeaponMovementRotation;
    Vector3 targetWeaponMovementRotationVelocity;

    [Header("Weapon Sway")]
    public Transform WeaponSwayObject;
    public float swayAmountA = 1;
    public float swayAmountB = 2;
    public float swayScale = 600;
    public float swayLerpSpeed = 14;
    public float swayTime;
    public Vector3 swayPosition;


    [Header("Sights")]
    public Transform sightTarget;
    public float sightOffset;
    public float aimingInTime;
    private Vector3 weaponSwayPosition;
    private Vector3 weaponSwayVelocity;
    [HideInInspector]
    public bool isAimingIn;


    private void Start()
    {
        newWeaponRotation = transform.localRotation.eulerAngles;
        motor = GetComponentInParent<PlayerMotor>();
        look = GetComponentInParent<PlayerLook>();
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
        CalculateAimingIn();
        CalculateWeaponSway();

    }
    private void CalculateAimingIn()
    {
        var targetPosition = transform.position;

        if (isAimingIn)
        {
            targetPosition = look.cam.transform.position + (WeaponSwayObject.transform.position - sightTarget.position) + (look.cam.transform.forward * sightOffset);
        }

        weaponSwayPosition = WeaponSwayObject.transform.position;
        weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayVelocity, aimingInTime);
        WeaponSwayObject.transform.position = weaponSwayPosition + swayPosition;

    }
    private void CalculateWeaponRotation()
    {
        weaponAnimator.speed = input.weaponAnimationSpeed;

        Vector2 rotationValue = input.onFoot.Look.ReadValue<Vector2>();
        targetWeaponRotation.y += (isAimingIn ? settings.swayAmount / 2 : settings.swayAmount) * (settings.swayXInverted ? -rotationValue.x : rotationValue.x) * Time.deltaTime;
        targetWeaponRotation.x += (isAimingIn ? settings.swayAmount / 2 : settings.swayAmount) * (settings.swayYInverted ? rotationValue.y * 2 : -rotationValue.y * 2) * Time.deltaTime;

        targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -settings.swayClampX, settings.swayClampX);
        targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -settings.swayClampY, settings.swayClampY);
        targetWeaponRotation.z = isAimingIn ? 0 : targetWeaponRotation.y;

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, settings.swayResetSmoothing);
        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, settings.swaySmoothing);

        Vector2 movementValue = input.onFoot.Movement.ReadValue<Vector2>();
        targetWeaponMovementRotation.z = (isAimingIn ? settings.movementSwayX / 3 : settings.movementSwayX) * (settings.movementSwayXInverted ? -movementValue.x : movementValue.x);
        targetWeaponMovementRotation.x = (isAimingIn ? settings.movementSwayY / 3 : settings.movementSwayY) * (settings.movementSwayYInverted ? -movementValue.y : movementValue.y);

        targetWeaponMovementRotation = Vector3.SmoothDamp(targetWeaponMovementRotation, Vector3.zero, ref targetWeaponMovementRotationVelocity, settings.movementSwaySmoothing);
        newWeaponMovementRotation = Vector3.SmoothDamp(newWeaponMovementRotation, targetWeaponMovementRotation, ref newWeaponMovementRotationVelocity, settings.movementSwaySmoothing);

        transform.localRotation = Quaternion.Euler(newWeaponRotation + newWeaponMovementRotation);
    }

    private void SetWeaponAnimations()
    {
        weaponAnimator.SetBool("isSprinting", motor.sprinting);
    }

    private void CalculateWeaponSway()
    {
        var targetPosition = LissajousCurve(swayTime, swayAmountA, swayAmountB) / (isAimingIn ? swayScale * 2 : swayScale);
        swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * swayLerpSpeed);
        swayTime += Time.deltaTime;

        if (swayTime > 6.3f)
        {
            swayTime = 0;
        }

    }
    private Vector3 LissajousCurve(float Time, float A, float B)
    {
        return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time + Mathf.PI));
    }

}
