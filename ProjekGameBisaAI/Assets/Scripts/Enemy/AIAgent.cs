using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;

    public AIWeapon weapon;
    public AiStateID initialState;
    public NavMeshAgent navMeshAgent;
    public AIAgentConfig config;
    public Ragdoll ragdoll;
    public Transform playerTransform;
    public GameObject enemyHealthBar;
    public GameObject player;
    public AISensor sensor;

    public WeaponIK weaponIK;

    public float rayCastDisctance = 40f;
    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        weapon = GetComponent<AIWeapon>();
        sensor = GetComponent<AISensor>();
        weaponIK = GetComponent<WeaponIK>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIAttackPlayerState());
        stateMachine.RegisterState(new AIPatrolingState());
        stateMachine.ChangeState(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
