using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Enemy enemy;
    public void OnRayCastHit(WeaponController weapon, Vector3 direction)
    {
        enemy.TakeDamage(weapon.damage, direction);
    }
}
