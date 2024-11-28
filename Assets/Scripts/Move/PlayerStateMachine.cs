using UnityEngine;
using UnityEngine.AI;

public class PlayerStateMachine : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    public LayerMask interactableLayer;

    private void Awake()
    {
        interactableLayer = LayerMask.GetMask("Ground") | LayerMask.GetMask("Enemy");
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        stateMachine = new StateMachine();
        stateMachine.ChangeState(new IdleState(gameObject, stateMachine, agent, interactableLayer));
    }

    private void Update()
    {
        stateMachine.Update();
    }
}