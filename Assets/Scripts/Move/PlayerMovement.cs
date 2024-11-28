using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    public LayerMask groundLayer; // 마우스 클릭 시 대상 레이어
    public NavMeshAgent agent; // NavMeshAgent 컴포넌트
    public float moveSpeed = 20f; // 이동 속도

    private Vector3 targetPosition;
    private bool isMoving = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        // NavMeshAgent의 기본 설정 비활성화
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    void Update()
    {
        // 마우스 우클릭으로 목표 지점 설정
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                targetPosition = hit.point;
                agent.SetDestination(targetPosition); // NavMeshAgent 경로 설정
                isMoving = true;
            }
        }

        // 이동 로직
        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 velocity = agent.desiredVelocity;
        velocity.y = 0; // Y축 제거

        // 목표 방향을 따라 빠르게 회전
        if (velocity.sqrMagnitude > 0.01f) // 이동 중인 경우에만 회전
        {
            float rotationSpeed = 1000f; // 빠른 회전 속도
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
