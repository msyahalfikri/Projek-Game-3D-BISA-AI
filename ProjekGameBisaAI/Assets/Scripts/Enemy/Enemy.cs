using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float enemyHealth = 100f;
    public GameObject player;
    private float lerpTimer;
    [Header(" Enemy Health bar")]
    public float enemyMaxHealth = 100f;
    public float chipSpeed = 2f;
    public Image bgHealthBar;
    public Image frontHealthBar;
    public Image backHealthBar;

    public void TakeDamage(float amount)
    {
        enemyHealth -= amount;
        lerpTimer = 0f;
        if (enemyHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
    private void Start()
    {
        enemyHealth = enemyMaxHealth;
    }
    private void Update()
    {
        enemyHealth = Mathf.Clamp(enemyHealth, 0, enemyMaxHealth);
        UpdateEnemyHealthUI();

        frontHealthBar.transform.LookAt(player.transform.position);
        backHealthBar.transform.LookAt(player.transform.position);
        bgHealthBar.transform.LookAt(player.transform.position);
    }
    public void UpdateEnemyHealthUI()
    {
        float eFillF = frontHealthBar.fillAmount;
        float eFillB = backHealthBar.fillAmount;
        float hFraction = enemyHealth / enemyMaxHealth;
        if (eFillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(eFillB, hFraction, percentComplete);
        }
    }
}
