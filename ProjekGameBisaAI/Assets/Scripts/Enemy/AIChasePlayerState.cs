using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChasePlayerState : AIState
{
    float timer = 0f;
    public AiStateID GetID()
    {
        return AiStateID.ChasePlayer;
    }
    public void Enter(AIAgent agent)
    {

        agent.weapon.EquipWeapon();
    }
    public void Update(AIAgent agent)
    {
        if (agent.weapon.HasWeapon() && agent.sensor.IsInSight(agent.playerTransform.gameObject))
        {
            agent.stateMachine.ChangeState(AiStateID.AttackPlayer);
        }
        timer -= Time.deltaTime;
        if (!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = agent.playerTransform.position - agent.navMeshAgent.destination;
        }

        if (timer < 0.0f)
        {
            Vector3 direction = (agent.playerTransform.position - agent.navMeshAgent.destination);
            direction.y = 0;
            if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }
            }
            timer = agent.config.maxTime;
        }

        if (!agent.enabled)
        {
            return;
        }

    }
    public void Exit(AIAgent agent)
    {

    }
}
