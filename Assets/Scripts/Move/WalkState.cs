using UnityEngine;
using UnityEngine.AI;

public class MoveState : BaseState
{
    private Vector3 targetPosition;
    private LayerMask interactableLayer;
    private GameObject targetEnemy;
    private float attackRange = 10f;
    public MoveState(GameObject player, StateMachine stateMachine, NavMeshAgent agent, Vector3 targetPosition, LayerMask interactableLayer, GameObject targetEnemy = null) : base(player, stateMachine, agent)
    {
        this.targetPosition = targetPosition;
        this.interactableLayer = interactableLayer;
        this.targetEnemy = targetEnemy;
    }

    public override void Enter()
    {
        if (targetEnemy != null)
        {
            targetPosition = targetEnemy.transform.position;
        }
        agent.SetDestination(targetPosition);
    }

    public override void Update()
    {
        HandleMoveInput();
        MovePlayer();
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
                    agent.SetDestination(targetPosition);
                }
                else
                {
                    targetEnemy = null;
                    targetPosition = hit.point;
                    agent.SetDestination(targetPosition);
                }
            }
        }
    }

    private void MovePlayer()
    {
        Vector3 velocity = agent.desiredVelocity;
        velocity.y = 0;

        if (velocity.sqrMagnitude > 0.01f)
        {
            float rotationSpeed = 1000f;
            player.transform.rotation = Quaternion.RotateTowards(
                player.transform.rotation,
                Quaternion.LookRotation(velocity),
                rotationSpeed * Time.deltaTime
            );

            player.transform.position += velocity * Time.deltaTime;
            agent.nextPosition = player.transform.position;
        }

        if (targetEnemy != null)
        {
            float distanceToEnemy = Vector3.Distance(player.transform.position, targetEnemy.transform.position);
            if (distanceToEnemy <= attackRange)
            {
                stateMachine.ChangeState(new AttackState(player, stateMachine, agent, interactableLayer, targetEnemy));
            }
        }
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            stateMachine.ChangeState(new IdleState(player, stateMachine, agent, interactableLayer));
        }
    }
}