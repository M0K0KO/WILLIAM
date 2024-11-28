using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : BaseState
{
    private LayerMask interactableLayer;
    public IdleState(GameObject player, StateMachine stateMachine, NavMeshAgent agent, LayerMask interactableLayer) : base(player, stateMachine, agent)
    {
        this.interactableLayer = interactableLayer;
    }

    private GameObject targetEnemy;
    private Vector3 targetPosition;

    public override void Enter()
    {
    }

    public override void Update()
    {
        HandleMoveInput();
    }

    public override void Exit()
    {
    }

    private void HandleMoveInput()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    targetEnemy = hit.collider.gameObject;
                    targetPosition = targetEnemy.transform.position;
                    stateMachine.ChangeState(new MoveState(player, stateMachine, agent, targetPosition, interactableLayer, targetEnemy));
                }
                else
                {
                    targetEnemy = null;
                    targetPosition = hit.point;
                    stateMachine.ChangeState(new MoveState(player, stateMachine, agent, targetPosition, interactableLayer));
                }
            }
        }
    }
}