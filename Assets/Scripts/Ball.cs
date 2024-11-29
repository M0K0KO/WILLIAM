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
        float radius = 2f; // �Ÿ�
        int pointCount = 8; // ���� ����
        float angleIncrement = 45f; // �� ���� �� ���� (�� ����)

        Vector3[] points = new Vector3[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleIncrement); // ������ �������� ��ȯ
            float x = radius * Mathf.Cos(angle); // x ��ǥ
            float z = radius * Mathf.Sin(angle); // z ��ǥ
            points[i] = new Vector3(x, 0, z); // y ��ǥ�� 0���� ����
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

        Vector3 direction = (end - start).normalized; // ���� ���
        Vector3 velocity = direction * v0 * Mathf.Cos(radianAngle);
        velocity.y = v0 * Mathf.Sin(radianAngle); // y �� �ӵ� �߰�

        return velocity;
    }

    private IEnumerator DestoryAfterTime()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}
