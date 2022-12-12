using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackPlayerState : AIState
{
    public Vector3 direction;


    public AiStateID GetID()
    {
        return AiStateID.AttackPlayer;
    }
    public void Enter(AIAgent agent)
    {
        agent.weapon.ActivateWeapon();
        agent.weapon.SetTarget(agent.playerTransform);
        // agent.weapon.SetFiring(true, agent.player, agent.rayCastDisctance);
        agent.navMeshAgent.stoppingDistance = 10.0f;

    }
    public void Update(AIAgent agent)
    {
        UpdateFiring(agent);
        // agent.weapon.SetFiring(true, agent.player, agent.rayCastDisctance);
        agent.navMeshAgent.destination = agent.playerTransform.position;
    }
    private void UpdateFiring(AIAgent agent)
    {
        if (agent.sensor.IsInSight(agent.playerTransform.gameObject))
        {
            agent.weapon.SetFiring(true, agent.player, agent.rayCastDisctance);
        }
        else
        {
            agent.weapon.SetFiring(false, agent.player, agent.rayCastDisctance);
            // agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
        }

    }
    public void Exit(AIAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = 0.0f;
    }
}
