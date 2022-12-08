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
        agent.navMeshAgent.stoppingDistance = 5.0f;
    }
    public void Update(AIAgent agent)
    {
        agent.navMeshAgent.destination = agent.playerTransform.position;
    }
    public void Exit(AIAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = 0.0f;
    }
}
