using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    public AiStateID GetID()
    {
        return AiStateID.Idle;
    }

    public void Enter(AIAgent agent)
    {

    }
    public void Update(AIAgent agent)
    {
        Vector3 playerDirection = agent.playerTransform.position - agent.transform.position;
        if (playerDirection.magnitude > agent.config.maxSightDisctance)
        {
            return;
        }

        Vector3 agentDirection = agent.transform.forward;
        playerDirection.Normalize();
        float dotProduct = Vector3.Dot(playerDirection, agentDirection);
        if (dotProduct > 0.0f)
        {
            agent.stateMachine.ChangeState(AiStateID.ChasePlayer);
        }

        // if (!agent.navMeshAgent.hasPath)
        // {
        //     agent.stateMachine.ChangeState(AiStateID.Patroling);
        // }
        // else
        // {
        //     agent.stateMachine.ChangeState(AiStateID.Idle);
        // }
    }
    public void Exit(AIAgent agent)
    {

    }
}
