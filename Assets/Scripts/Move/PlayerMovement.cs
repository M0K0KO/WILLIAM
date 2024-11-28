using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    public LayerMask groundLayer; // ���콺 Ŭ�� �� ��� ���̾�
    public NavMeshAgent agent; // NavMeshAgent ������Ʈ
    public float moveSpeed = 20f; // �̵� �ӵ�

    private Vector3 targetPosition;
    private bool isMoving = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        // NavMeshAgent�� �⺻ ���� ��Ȱ��ȭ
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    void Update()
    {
        // ���콺 ��Ŭ������ ��ǥ ���� ����
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                targetPosition = hit.point;
                agent.SetDestination(targetPosition); // NavMeshAgent ��� ����
                isMoving = true;
            }
        }

        // �̵� ����
        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 velocity = agent.desiredVelocity;
        velocity.y = 0; // Y�� ����

        // ��ǥ ������ ���� ������ ȸ��
        if (velocity.sqrMagnitude > 0.01f) // �̵� ���� ��쿡�� ȸ��
        {
            float rotationSpeed = 1000f; // ���� ȸ�� �ӵ�
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(velocity),
                rotationSpeed * Time.deltaTime
            );

            transform.position += velocity * Time.deltaTime;

            agent.nextPosition = transform.position;
        }
    }


}
