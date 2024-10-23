using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class LightingBoid : MonoBehaviour
{
    public Collider2D trigger;
    public CircleCollider2D circleCollider;
    private Rigidbody2D rb;

    [Header("基本属性")]
    public BoidParent parent;
    public float radius;
    public float flySpeed;
    [SerializeField] private LightingBoidStatues statues = LightingBoidStatues.Cruise;

    private List<LightingBoid> allBoids;
    public Vector2 initialDirection;
    private Vector2 finalDirection;
    private Tween doAnimate;
    private Sequence doAnimates;

    [Header("待机相关")]
    public float cruiseRadius = 2f;
    public float idleTime = 0.75f;
    public float cruiseDuration = 1f;
    private Vector3 center;
    [SerializeField] private Vector3 cruisePosition;
    private bool isCrusing = false;
    private float idleCounter;

    [Header("视野属性")]
    public float visionRadius = 5.0f; // 视野半径
    public float visionAngle = 60.0f;  // 视场角度（度）
    private float visionConeThreshold; // 视场

    [Header("鸟群模拟_区域")]
    public float m_separationRadius; // 分离判定半径
    public float m_alignmentRadius; // 对齐判定半径
    public float m_cohesionRadius; // 聚合判定半径
    public float m_avoidanceRadius; // 避障判定半径

    [Header("鸟群模拟_权重")]
    public float separationWeight = 2.5f;
    public float alignmentWeight = 1.0f;
    public float cohesionWeight = 1.0f;
    public float avoidanceWeight = 1.5f;
    public float parentWeight = 1.0f;

    [Header("攻击相关")]
    public float reBoundSpeed;
    public float reBoundTime;
    public float reBoundShrinkCoef = 0.8f;
    public float cuvature = 1f;
    public float percentSpeed = 0.00005f;
    [SerializeField] private bool isTracking;
    private Transform targetTransform;
    private float targetRadius;
    private Vector3 originPoint;
    private Vector3 controlPoint;
    private float percent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        visionConeThreshold = Mathf.Cos(visionAngle * 0.5f * Mathf.Deg2Rad);
        SetStatuesCruise();

        if (circleCollider != null)
            radius = circleCollider.radius * Mathf.Max(transform.localScale.x, transform.localScale.y);
    }

    private void FixedUpdate()
    {
        switch (statues)
        {
            case LightingBoidStatues.Cruise:
                BoidCruise();
                break;
            case LightingBoidStatues.Follow:
                BoidFollow();
                break;
            case LightingBoidStatues.Attack:
                if (isTracking)
                    OnTrack();
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
        ActionSwitching();
        statues = newStaues;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (statues == LightingBoidStatues.Follow)
            return;

        if (collision.GetComponent<BoidParent>() != null)
            SetParent(collision.GetComponent<BoidParent>());
    }

    private void BoidDestroy()
    {
        ActionSwitching();

        parent.DeleteBoid(this);
        gameObject.SetActive(false);
    }

    private void ActionSwitching()
    {
        if (doAnimate != null)
            doAnimate.Kill();
        if (doAnimates != null)
            doAnimates.Kill();

        rb.velocity = Vector3.zero;
    }

    #region 巡游逻辑

    private void BoidCruise()
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

    public void SetStatuesCruise()
    {
        statues = LightingBoidStatues.Cruise;
        center = transform.position;
        isCrusing = false;
    }

    #endregion

    #region 跟随逻辑

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
        Vector2 myDirection = initialDirection; // 这里假设物体的正上方是它的前方 之后会设定为跟随主角 需要修改
        Vector2 toTarget = (targetPosition - (Vector2)transform.position).normalized;

        // 点积计算
        float dotProduct = Vector2.Dot(myDirection, toTarget);

        // 检查点积是否大于阈值
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

        // 查找在范围内并且在视野锥内的对象
        foreach (var b in allBoids)
        {
            if (b != this
                && (b.transform.position - transform.position).magnitude <= m_separationRadius
                && InVisionCone(b.transform.position))
            {
                boidsInRange.Add(b);
            }
        }

        // 计算分离方向
        foreach (var boid in boidsInRange)
        {
            float ratio = Mathf.Clamp01((boid.transform.position - transform.position).magnitude / m_separationRadius);
            direction -= ratio * (Vector2)(boid.transform.position - transform.position);
        }

        return direction.normalized; // 返回最终的分离方向向量 要做归一化
    }

    private Vector2 SteerAlignment()
    {
        Vector2 alignmentDirection = Vector2.zero; // 用于存储对齐方向
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

        // 累加邻居方向
        foreach (var boid in boidsInRange)
        {
            alignmentDirection += boid.finalDirection; // 累加邻居的朝向向量 之后需要修改
        }

        if (boidsInRange.Count > 0)
        {
            alignmentDirection /= boidsInRange.Count; // 计算平均方向
            alignmentDirection = alignmentDirection.normalized; // 归一化向量
        }

        return alignmentDirection.normalized; // 返回对齐方向
    }

    private Vector2 SteerCohesion()
    {
        Vector2 cohesionDirection = Vector2.zero; // 用于存储聚合方向
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
                centerOfMass += (Vector2)boid.transform.position; // 累加位置
            }
            centerOfMass /= boidsInRange.Count; // 计算质心
            cohesionDirection = (centerOfMass - (Vector2)transform.position).normalized; // 计算方向并归一化
        }

        return cohesionDirection.normalized; // 返回朝向聚合中心的方向
    }

    private Vector2 SteerColliders()
    {
        Vector2 avoidDirection = Vector2.zero; // 用于存储避障方向

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
    //    // 设置 Gizmo 颜色
    //    Color originalColor = Gizmos.color;

    //    // 绘制分离距离的 Gizmo
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, m_separationRadius);

    //    // 绘制对齐距离的 Gizmo
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, m_alignmentRadius);

    //    // 绘制聚合距离的 Gizmo
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, m_cohesionRadius);

    //    // 绘制与父物体的距离的 Gizmo
    //    //Gizmos.color = Color.yellow;
    //    //Vector3 parentPosition = parent.transform.position;
    //    //Gizmos.DrawLine(transform.position, parentPosition);
    //    //Gizmos.DrawWireSphere(transform.position, Vector2.Distance(transform.position, parentPosition));

    //    // 恢复 Gizmo 原始颜色
    //    Gizmos.color = originalColor;
    //}
    #endregion

    #region 进攻逻辑

    public void StartAttack(Transform enemyTrans)
    {
        targetTransform = enemyTrans;
        targetRadius = enemyTrans.GetComponent<BlackBoid>().ReturnRadius();
        SetBoidStatues(parent, LightingBoidStatues.Attack);
        isTracking = true;

        originPoint = transform.position;
        controlPoint = GetMiddlePosition(transform.position, targetTransform.position);
    }

    private void OnAttack()
    {
        BlackBoid blackBoid = targetTransform.gameObject.GetComponent<BlackBoid>();
        blackBoid.GetAttack();
        ActionSwitching();
        isTracking = false;

        OnRebound(targetTransform);
    }

    private void OnRebound(Transform targetTrans)
    {
        Vector3 reBoundDir = (transform.position - targetTrans.position).normalized;
        Vector3 reBoundEndPos1 = transform.position + reBoundDir * reBoundSpeed * reBoundTime * 0.8f;
        Vector3 reBoundEndPos2 = reBoundEndPos1 + reBoundDir * reBoundSpeed * reBoundTime * 0.2f;

        Sequence reBoundSequence = DOTween.Sequence();
        doAnimates = reBoundSequence;
        Vector3 originScale = transform.localScale;
        Vector3 shrinkScale = transform.localScale * reBoundShrinkCoef;

        reBoundSequence.Append(transform.DOScale(shrinkScale, 0.05f).SetEase(Ease.OutBounce));
        reBoundSequence.Append(transform.DOScale(originScale, 0.1f).SetEase(Ease.OutBounce));
        reBoundSequence.Join(transform.DOMove(reBoundEndPos1, 0.2f * reBoundTime));
        reBoundSequence.Append(transform.DOMove(reBoundEndPos2, 0.8f * reBoundTime)).OnComplete(BoidDestroy);
    }

    private void OnTrack()
    {
        if (statues != LightingBoidStatues.Attack)
            return;

        Vector3 direction = (transform.position - targetTransform.position).normalized;
        float distance = (transform.position - targetTransform.position).magnitude;
        float radiusSum = radius + targetRadius;
        if (distance > radiusSum)
        {
            percent += percentSpeed * Time.deltaTime;
            if (percent > 1)
            {
                percent = 1;
                statues = LightingBoidStatues.Cruise;
            }
            transform.position = Bezier(percent, originPoint, controlPoint, targetTransform.position);
        }
        else
        {
            transform.position = targetTransform.position + direction * radiusSum;
            OnAttack();
        }
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

