using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Ball : MonoBehaviour
{
    public Vector3 fallPosition;
    public Vector3 playerPosition;
    public Vector3 TargetPosition { get; set; }
    public float Speed = 20f;
    public float launchAngle = 50f;

    private bool isBounced = false;
    private Rigidbody rb;

    private void Start()
    {
        Vector3 origin = Vector3.zero;
        float radius = 2f; // 거리
        int pointCount = 8; // 지점 개수
        float angleIncrement = 45f; // 각 지점 간 간격 (도 단위)

        Vector3[] points = new Vector3[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleIncrement); // 각도를 라디안으로 변환
            float x = radius * Mathf.Cos(angle); // x 좌표
            float z = radius * Mathf.Sin(angle); // z 좌표
            points[i] = new Vector3(x, 0, z); // y 좌표는 0으로 고정
        }

        Vector3 randomPoint = points[Random.Range(0, pointCount)];
        fallPosition = playerPosition + randomPoint;

        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void Update()
    {
        if (TargetPosition != null && isBounced == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, Speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            isBounced = true;

            Vector3 initialVelocity = CalculateLaunchVelocity(TargetPosition, fallPosition, launchAngle);

            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = initialVelocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            StartCoroutine(DestoryAfterTime());
        }
    }

    private Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 end, float angle)
    {
        float gravity = Mathf.Abs(Physics.gravity.y);

        float distance = Vector3.Distance(new Vector3(start.x, 0, start.z), new Vector3(end.x, 0, end.z));
        float heightDifference = end.y - start.y;

        float radianAngle = Mathf.Deg2Rad * angle;

        float v0Squared = (gravity * distance * distance) /
                          (2 * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle) *
                          (distance * Mathf.Tan(radianAngle) - heightDifference));

        if (v0Squared <= 0)
        {
            Debug.LogError("Invalid parameters for projectile motion. Adjust start/end positions or angle.");
            return Vector3.zero;
        }

        float v0 = Mathf.Sqrt(v0Squared);

        Vector3 direction = (end - start).normalized; // 방향 계산
        Vector3 velocity = direction * v0 * Mathf.Cos(radianAngle);
        velocity.y = v0 * Mathf.Sin(radianAngle); // y 축 속도 추가

        return velocity;
    }

    private IEnumerator DestoryAfterTime()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}
