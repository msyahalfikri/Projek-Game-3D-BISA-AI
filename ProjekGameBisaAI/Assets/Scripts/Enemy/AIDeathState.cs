using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    public Vector3 direction;

    public AiStateID GetID()
    {
        return AiStateID.Death;
    }
    public void Enter(AIAgent agent)
    {
        agent.navMeshAgent.enabled = false;
        agent.weaponIK.enabled = false;
        agent.ragdoll.ActivateRagdoll();
        direction.y = 1;
        agent.ragdoll.ApplyForce(direction * agent.config.dieForce);
        agent.enemyHealthBar.SetActive(false);
        agent.weapon.DropWeapon();
    }
    public void Update(AIAgent agent)
    {

    }
    public void Exit(AIAgent agent)
    {

    }
}
