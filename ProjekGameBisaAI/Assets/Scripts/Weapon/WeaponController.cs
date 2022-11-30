using static scr_model;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{


    [Header("References")]
    public Animator weaponAnimator;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    private InputManager input;
    private PlayerMotor motor;

    private PlayerInteract playerInteract;

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

    [Header("Shooting")]
    public float rateOfFire = 15f;
    private float currentFireRate;
    public List<weaponFireType> allowedFireTypes;
    public weaponFireType currentFireType;
    private float nextTimetoFire = 0f;
    [HideInInspector]
    public bool isShooting;
    public float damage = 10f;
    public float range = 100f;
    public ParticleSystem muzzleFlash;

    public bool isReloading = false;
    public float reloadTime = 3f;
    public float fullMag = 30f;
    public float ammoInMag = 30;
    public float totalAmmo = 100;
    public float fullTotalAmmo = 100;
    int weaponIndex = 0;

    public TextMeshProUGUI ammoInMagText;
    public TextMeshProUGUI ammoTotalText;
    public TextMeshProUGUI weaponNameText;

    public Image weaponSilhouette;
    public List<Sprite> imageObjs;
    private void Start()
    {
        updateWeaponUI();
        newWeaponRotation = transform.localRotation.eulerAngles;
        motor = GetComponentInParent<PlayerMotor>();
        look = GetComponentInParent<PlayerLook>();
        currentFireType = allowedFireTypes.First();
    }
    public void initialize(InputManager inputManager)
    {
        input = inputManager;
        isInitialized = true;
    }
    private void Update()
    {
        if (isReloading)
        {
            return;
        }
        if (ammoInMag <= 0)
        {
            StartCoroutine(ReloadWeapon());
            return;
        }

        if (!isInitialized)
        {
            return;
        }
        CalculateWeaponRotation();
        SetWeaponAnimations();
        CalculateAimingIn();
        CalculateWeaponSway();
        CalculateShooting();
        weaponIndex = input.weaponIndex;

    }
    private void CalculateAimingIn()
    {
        var targetPosition = transform.position;

        if (!motor.sprinting)
        {
            if (isAimingIn)
            {
                targetPosition = look.cam.transform.position + (WeaponSwayObject.transform.position - sightTarget.position) + (look.cam.transform.forward * sightOffset);
                motor.speed = 2.5f;
            }
            else
            {
                motor.speed = 5f;
            }

            weaponSwayPosition = WeaponSwayObject.transform.position;
            weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayVelocity, aimingInTime);
            WeaponSwayObject.transform.position = weaponSwayPosition + swayPosition;
        }


    }
    private void CalculateWeaponRotation()
    {

        weaponAnimator.speed = 1f;

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

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawn);
        muzzleFlash.Play();
        ammoInMag--;
        updateWeaponUI();

        Ray ray = new Ray(look.cam.transform.position, look.cam.transform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, range))
        {
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

    }

    private void CalculateShooting()
    {
        if (isReloading)
        {
            return;
        }
        if (!motor.sprinting)
        {
            if (ammoInMag != 0)
            {
                if (isShooting)
                {
                    weaponAnimator.SetTrigger("isShooting");


                    if (currentFireType == weaponFireType.semiAuto)
                    {
                        Shoot();
                        isShooting = false;

                    }
                    else if (currentFireType == weaponFireType.FullyAuto && Time.time >= nextTimetoFire)
                    {
                        nextTimetoFire = Time.time + 1f / rateOfFire;
                        Shoot();
                    }
                }
            }

        }

    }

    public void updateWeaponUI()
    {

        if (weaponIndex == 0)
        {
            weaponNameText.text = "Stengun";

        }
        else if (weaponIndex == 1)
        {
            weaponNameText.text = "M1911";

        }
        weaponSilhouette.sprite = imageObjs[weaponIndex];

        ammoInMagText.text = ammoInMag.ToString();
        ammoTotalText.text = totalAmmo.ToString();
    }

    public IEnumerator ReloadWeapon()
    {
        if (!isReloading)
        {
            if (ammoInMag < fullMag)
            {
                if (!motor.sprinting)
                {
                    if (totalAmmo != 0)
                    {
                        isReloading = true;
                        weaponAnimator.SetTrigger("isReloading");
                        yield return new WaitForSeconds(reloadTime);
                        if (totalAmmo >= fullMag)
                        {
                            float reloadedAmmoMore = fullMag - ammoInMag;
                            ammoInMag += reloadedAmmoMore;
                            totalAmmo -= reloadedAmmoMore;
                        }
                        else if (totalAmmo < fullMag)
                        {
                            float reloadedAmmoLess = fullMag - ammoInMag;

                            if (totalAmmo >= reloadedAmmoLess)
                            {
                                ammoInMag += reloadedAmmoLess;
                                totalAmmo -= reloadedAmmoLess;
                            }
                            else
                            {
                                ammoInMag += totalAmmo;
                                totalAmmo -= totalAmmo;
                            }

                        }
                        updateWeaponUI();
                        isReloading = false;

                    }
                }
            }
        }
    }

    public void RefillAmmo()
    {
        totalAmmo = fullTotalAmmo;
        updateWeaponUI();
    }

}
