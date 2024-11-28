using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : BaseState
{
    private GameObject targetEnemy;
    private float attackInterval = 0.5f;
    private float attackTimer;
    private LayerMask interactableLayer;
    private GameObject projectilePrefab;
    private Transform playerTransform;
    public AttackState(GameObject player, StateMachine stateMachine, NavMeshAgent agent, LayerMask interactableLayer, GameObject targetEnemy) : base(player, stateMachine, agent)
    {
        this.interactableLayer = interactableLayer;
        this.targetEnemy = targetEnemy;
        this.playerTransform = player.transform;
    }

    private Vector3 targetPosition;

    public override void Enter()
    {
        agent.isStopped = true;
        projectilePrefab = Resources.Load<GameObject>("Ball");
        attackTimer = 0.1f;
    }

    public override void Update()
    {
        HandleInput();

        if (targetEnemy == null)
        {
            stateMachine.ChangeState(new IdleState(player, stateMachine, agent, interactableLayer));
            return;
        }

        // ������ �Ÿ� Ȯ��
        float distanceToEnemy = Vector3.Distance(player.transform.position, targetEnemy.transform.position);
        if (distanceToEnemy > 10f) // ��Ÿ� �ʰ� �� �̵� ���·� ��ȯ
        {
            stateMachine.ChangeState(new MoveState(player, stateMachine, agent, targetEnemy.transform.position, interactableLayer, targetEnemy));
            return;
        }

        // ���� ����
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            Attack();
            attackTimer = 0f;
        }
    }

    public override void Exit()
    {
        agent.isStopped = false;
    }

    private void Attack()
    {
        Debug.Log($"{player.name}�� {targetEnemy.name}�� ������");
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    GameObject clickedEnemy = hit.collider.gameObject;

                    if (clickedEnemy != targetEnemy)
                    {
                        targetPosition = clickedEnemy.transform.position;
                        stateMachine.ChangeState(new MoveState(player, stateMachine, agent, targetPosition, interactableLayer, targetEnemy));
                    }
                }
                else
                {
                    targetEnemy = null;
                    stateMachine.ChangeState(new MoveState(player, stateMachine, agent, targetPosition, interactableLayer));
                }
            }
        }
    }
}