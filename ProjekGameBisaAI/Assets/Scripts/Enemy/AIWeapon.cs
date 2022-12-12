using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIWeapon : MonoBehaviour
{
    public WeaponController weaponFab;
    public WeaponController currentWeapon;
    Animator animator;
    MeshSockets sockets;
    WeaponIK weaponIK;
    Transform currentTarget;
    bool weaponActive = false;
    public int innaccuracy = 10;


    private void Update()
    {

    }
    public void SetFiring(bool enabled, GameObject player, float rayCastDisctance)
    {
        if (enabled && weaponActive)
        {
            currentWeapon.isShooting = true;
            currentWeapon.EnemyCalculateShooting(innaccuracy, player, rayCastDisctance);
        }
        else
        {
            currentWeapon.EnemyStopShoot();
            currentWeapon.EnemyCalculateShooting(innaccuracy, player, rayCastDisctance);
        }
    }
    public void EquipWeapon()
    {
        WeaponController enemyWeapon = Instantiate(weaponFab);
        currentWeapon = enemyWeapon;
        sockets.Attach(currentWeapon.transform, MeshSockets.SocketID.Spine);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        sockets = GetComponent<MeshSockets>();
        weaponIK = GetComponent<WeaponIK>();
    }

    public void ActivateWeapon()
    {
        StartCoroutine(PullOutWeapon());
    }

    IEnumerator PullOutWeapon()
    {
        animator.SetBool("Equip", true);
        yield return new WaitForSeconds(0.5f);
        while (animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
        {
            yield return null;
        }

        weaponIK.SetAimTransform(currentWeapon.raycastOrigin);
        weaponActive = true;
    }
    public void DropWeapon()
    {
        if (currentWeapon)
        {
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.gameObject.AddComponent<Rigidbody>();
            currentWeapon = null;
        }
    }

    public bool HasWeapon()
    {
        return currentWeapon != null;
    }

    public void OnAnimationEvent(string eventName)
    {
        if (eventName == "EquipWeapon")
        {
            sockets.Attach(currentWeapon.transform, MeshSockets.SocketID.RightHand);
        }
    }


    public void SetTarget(Transform target)
    {
        weaponIK.SetTargetTransform(target);
        currentTarget = target;
    }
}
