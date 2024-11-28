using UnityEngine;
using UnityEngine.AI;

public abstract class BaseState
{
    protected GameObject player;
    protected StateMachine stateMachine;
    protected NavMeshAgent agent;

    public BaseState(GameObject player, StateMachine stateMachine, NavMeshAgent agent)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.agent = agent;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}