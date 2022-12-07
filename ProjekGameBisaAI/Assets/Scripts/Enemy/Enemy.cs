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
    AIAgent agent;

    public void TakeDamage(float amount, Vector3 direction)
    {
        enemyHealth -= amount;
        lerpTimer = 0f;
        if (enemyHealth <= 0f)
        {
            Die(direction);
        }
    }

    private void Die(Vector3 direction)
    {
        AIDeathState deathState = agent.stateMachine.GetState(AiStateID.Death) as AIDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChangeState(AiStateID.Death);
    }
    private void Start()
    {
        enemyHealth = enemyMaxHealth;
        agent = GetComponent<AIAgent>();

        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidbodies)
        {
            Hitbox hitbox = rigidBody.gameObject.AddComponent<Hitbox>();
            hitbox.enemy = this;
        }
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
