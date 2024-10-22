using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class LightingBoid : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D trigger;

    [Header("��������")]
    public BoidParent parent;
    [SerializeField] private List<LightingBoid> allBoids;
    public float flySpeed;
    [SerializeField] private LightingBoidStatues statues = LightingBoidStatues.Idle;

    public Vector2 initialDirection;
    private Vector2 finalDirection;
    private Tween doAnimate;

    [Header("�������")]
    public float cruiseRadius = 2f;
    public float idleTime = 0.75f;
    public float cruiseDuration = 1f;
    private Vector3 center;
    [SerializeField] private Vector3 cruisePosition;
    private bool isCrusing = false;
    private float idleCounter;

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

    [Header("�������")]
    public float cuvature = 1f;
    public float percentSpeed = 0.00005f;
    private Transform targetTransform;
    private Vector3 originPoint;
    private Vector3 controlPoint;
    [SerializeField] private float percent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        visionConeThreshold = Mathf.Cos(visionAngle * 0.5f * Mathf.Deg2Rad);
        SetStatuesIdle();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        switch (statues)
        {
            case LightingBoidStatues.Idle:
                BoidIdle();
                break;
            case LightingBoidStatues.Follow:
                BoidFollow();
                break;
            case LightingBoidStatues.Attack:
                BoidAttack();
                break;
        }
    }

    public void SetParent(BoidParent parent)
    {
        this.parent = parent;
        parent.GetBoid(this);
        SetBoidStatues(parent, LightingBoidStatues.Follow);
    }

    public void UpdateBoidsList(List<LightingBoid> boidsList)
    {
        allBoids = boidsList;
    }

    public void SetBoidStatues(BoidParent parent, LightingBoidStatues newStaues)
    {
        if(doAnimate != null)
            doAnimate.Kill();
        statues = newStaues;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (statues == LightingBoidStatues.Follow)
            return;

        if (collision.GetComponent<BoidParent>() != null)
            SetParent(collision.GetComponent<BoidParent>());
    }

    public void AttackStart(Transform enemyTrans)
    {
        targetTransform = enemyTrans;
        SetBoidStatues(parent, LightingBoidStatues.Attack);

        originPoint = transform.position;
        controlPoint = GetMiddlePosition(transform.position, targetTransform.position);
    }

    #region �����߼�

    private void BoidIdle()
    {
        if(!isCrusing)
        {
            SetTargetPosition();
            isCrusing = true;
        }
        else
        {
            if (VectorApproximate.Approximate(transform.position, cruisePosition))
            {
                idleCounter += Time.deltaTime;
                if (idleCounter >= idleTime)
                {
                    isCrusing = false;
                    idleCounter = 0;
                }
            }
            else
                doAnimate = transform.DOMove(cruisePosition, cruiseDuration);
        }
    }
    private void SetTargetPosition()
    {
        float randomAngle = Random.Range(0f, 360f);

        float x = center.x + cruiseRadius * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        float y = center.y + cruiseRadius * Mathf.Sin(randomAngle * Mathf.Deg2Rad);

        cruisePosition = new Vector2(x, y);
    }

    public void SetStatuesIdle()
    {
        statues = LightingBoidStatues.Idle;
        center = transform.position;
        isCrusing = false;
    }

    #endregion

    #region �����߼�

    private void BoidFollow()
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

    //private void OnDrawGizmos()
    //{
    //    // ���� Gizmo ��ɫ
    //    Color originalColor = Gizmos.color;

    //    // ���Ʒ������� Gizmo
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, m_separationRadius);

    //    // ���ƶ������� Gizmo
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, m_alignmentRadius);

    //    // ���ƾۺϾ���� Gizmo
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, m_cohesionRadius);

    //    // �����븸����ľ���� Gizmo
    //    //Gizmos.color = Color.yellow;
    //    //Vector3 parentPosition = parent.transform.position;
    //    //Gizmos.DrawLine(transform.position, parentPosition);
    //    //Gizmos.DrawWireSphere(transform.position, Vector2.Distance(transform.position, parentPosition));

    //    // �ָ� Gizmo ԭʼ��ɫ
    //    Gizmos.color = originalColor;
    //}
    #endregion

    #region �����߼�

    private void BoidAttack()
    {
        if (statues != LightingBoidStatues.Attack)
            return;

        percent += percentSpeed * Time.deltaTime;
        if (percent > 1)
        {
            percent = 1;
            statues = LightingBoidStatues.Idle;
        }
        transform.position = Bezier(percent,originPoint,controlPoint,targetTransform.position);
    }

    private Vector3 GetMiddlePosition(Vector3 p0, Vector3 p2)
    {
        Vector3 middlePoint = Vector3.Lerp(p0, p2, 0.5f);
        Vector3 normal = Vector2.Perpendicular(p0 - p2).normalized;
        float randomDirection = Random.Range(-2f, 2f);
        return middlePoint + (p2 - p0).magnitude * cuvature * randomDirection * normal;
    }

    private Vector2 Bezier(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        var p12 = Vector3.Lerp(p0, p1, t);
        var p23 = Vector3.Lerp(p1, p2, t);
        return Vector3.Lerp(p12, p23, t);
    }

    #endregion

}

