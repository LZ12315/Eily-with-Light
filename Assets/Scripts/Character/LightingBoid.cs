using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBoid : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D trigger;

    [Header("��������")]
    public BoidParent parent;
    public float flySpeed;
    private bool isActive = false;
    public Vector2 initialDirection;
    private Vector2 finalDirection;
    [SerializeField] private List<LightingBoid> allBoids;

    [Header("��Ұ����")]
    public float visionRadius = 5.0f; // ��Ұ�뾶
    public float visionAngle = 60.0f;  // �ӳ��Ƕȣ��ȣ�
    private float visionConeThreshold; // �ӳ�

    [Header("��Ⱥģ��_����")]
    public float m_separationRadius; // �����ж��뾶
    public float m_alignmentRadius; // �����ж��뾶
    public float m_cohesionRadius; // �ۺ��ж��뾶
    public float m_avoidanceRadius; // �����ж��뾶

    [Header("��Ⱥģ��_Ȩ��")]
    public float separationWeight = 2.5f;
    public float alignmentWeight = 1.0f;
    public float cohesionWeight = 1.0f;
    public float avoidanceWeight = 1.5f;
    public float parentWeight = 1.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        visionConeThreshold = Mathf.Cos(visionAngle * 0.5f * Mathf.Deg2Rad);
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if(isActive)
            SteerActions();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive)
            return;
        if(collision.GetComponent<BoidParent>() != null)
        {
            parent = collision.GetComponent<BoidParent>();
            parent.GetBoid(this);
            isActive = true;
        }
    }

    private void SteerActions()
    {
        initialDirection = ParentLock();
        Vector2 separationDirection = SteerSeparation();
        Vector2 alignmentDirection = SteerAlignment();
        Vector2 cohesionDirection = SteerCohesion();
        Vector2 avoidanceDirection = SteerColliders();

        finalDirection = separationDirection * separationWeight +
                         alignmentDirection * alignmentWeight +
                         cohesionDirection * cohesionWeight +
                         avoidanceDirection * avoidanceWeight +
                         initialDirection * parentWeight;

        if (finalDirection != Vector2.zero)
        {
            finalDirection.Normalize();
        }

        rb.velocity = Vector2.Lerp(rb.velocity, finalDirection * flySpeed, Time.deltaTime);
    }

    public void UpdateBoidsList(List<LightingBoid> boidsList)
    {
        allBoids = boidsList;
    }

    #region �����߼�

    private bool InVisionCone(Vector2 targetPosition)
    {
        Vector2 myDirection = initialDirection; // ���������������Ϸ�������ǰ�� ֮����趨Ϊ�������� ��Ҫ�޸�
        Vector2 toTarget = (targetPosition - (Vector2)transform.position).normalized;

        // �������
        float dotProduct = Vector2.Dot(myDirection, toTarget);

        // ������Ƿ������ֵ
        return dotProduct > visionConeThreshold
               && (targetPosition - (Vector2)transform.position).magnitude <= visionRadius;
    }

    private Vector2 ParentLock()
    {
        if(parent != null)
            return (parent.transform.position - transform.position).normalized;
        else 
            return transform.position;
    }

    private Vector2 SteerSeparation()
    {
        Vector2 direction = Vector2.zero;
        var boidsInRange = new List<LightingBoid>();

        // �����ڷ�Χ�ڲ�������Ұ׶�ڵĶ���
        foreach (var b in allBoids)
        {
            if (b != this
                && (b.transform.position - transform.position).magnitude <= m_separationRadius
                && InVisionCone(b.transform.position))
            {
                boidsInRange.Add(b);
            }
        }

        // ������뷽��
        foreach (var boid in boidsInRange)
        {
            float ratio = Mathf.Clamp01((boid.transform.position - transform.position).magnitude / m_separationRadius);
            direction -= ratio * (Vector2)(boid.transform.position - transform.position);
        }

        return direction.normalized; // �������յķ��뷽������ Ҫ����һ��
    }

    private Vector2 SteerAlignment()
    {
        Vector2 alignmentDirection = Vector2.zero; // ���ڴ洢���뷽��
        var boidsInRange = new List<LightingBoid>();

        foreach (var boid in allBoids)
        {
            if (boid != this
                && (boid.transform.position - transform.position).magnitude <= m_alignmentRadius
                && InVisionCone(boid.transform.position))
            {
                boidsInRange.Add(boid);
            }
        }

        // �ۼ��ھӷ���
        foreach (var boid in boidsInRange)
        {
            alignmentDirection += boid.finalDirection; // �ۼ��ھӵĳ������� ֮����Ҫ�޸�
        }

        if (boidsInRange.Count > 0)
        {
            alignmentDirection /= boidsInRange.Count; // ����ƽ������
            alignmentDirection = alignmentDirection.normalized; // ��һ������
        }

        return alignmentDirection.normalized; // ���ض��뷽��
    }

    private Vector2 SteerCohesion()
    {
        Vector2 cohesionDirection = Vector2.zero; // ���ڴ洢�ۺϷ���
        var boidsInRange = new List<LightingBoid>();

        foreach (var boid in allBoids)
        {
            if (boid != this
                && (boid.transform.position - transform.position).magnitude <= m_cohesionRadius
                && InVisionCone(boid.transform.position))
            {
                boidsInRange.Add(boid);
            }
        }

        Vector2 centerOfMass = Vector2.zero;

        if (boidsInRange.Count > 0)
        {
            foreach (var boid in boidsInRange)
            {
                centerOfMass += (Vector2)boid.transform.position; // �ۼ�λ��
            }
            centerOfMass /= boidsInRange.Count; // ��������
            cohesionDirection = (centerOfMass - (Vector2)transform.position).normalized; // ���㷽�򲢹�һ��
        }

        return cohesionDirection.normalized; // ���س���ۺ����ĵķ���
    }

    private Vector2 SteerColliders()
    {
        Vector2 avoidDirection = Vector2.zero; // ���ڴ洢���Ϸ���

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, m_avoidanceRadius);

        foreach (var hitCollider in hitColliders)
        {
            var diff = (hitCollider.transform.position - transform.position);
            avoidDirection -= (Vector2)(diff.normalized / diff.sqrMagnitude);
        }

        return avoidDirection.normalized;
    }

    private void OnDrawGizmos()
    {
        // ���� Gizmo ��ɫ
        Color originalColor = Gizmos.color;

        // ���Ʒ������� Gizmo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_separationRadius);

        // ���ƶ������� Gizmo
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_alignmentRadius);

        // ���ƾۺϾ���� Gizmo
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, m_cohesionRadius);

        // �����븸����ľ���� Gizmo
        //Gizmos.color = Color.yellow;
        //Vector3 parentPosition = parent.transform.position;
        //Gizmos.DrawLine(transform.position, parentPosition);
        //Gizmos.DrawWireSphere(transform.position, Vector2.Distance(transform.position, parentPosition));

        // �ָ� Gizmo ԭʼ��ɫ
        Gizmos.color = originalColor;
    }
    #endregion

}

