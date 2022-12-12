using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrolingState : AIState
{
    public AiStateID GetID()
    {
        return AiStateID.Patroling;
    }
    public void Enter(AIAgent agent)
    {

    }
    public void Update(AIAgent agent)
    {
        if (!agent.navMeshAgent.hasPath)
        {
            WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();
            Vector3 min = worldBounds.min.position;
            Vector3 max = worldBounds.max.position;
            Vector3 randomPosition = new Vector3(
               Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                  Random.Range(min.z, max.z)
            );

            agent.navMeshAgent.destination = randomPosition;
        }
    }
    public void Exit(AIAgent agent)
    {

    }
}
