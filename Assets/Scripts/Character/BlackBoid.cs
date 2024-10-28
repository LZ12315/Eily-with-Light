using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoid : MonoBehaviour
{
    public CircleCollider2D circleCollider;
    private float radius;

    [Header(" ‹…À¬ﬂº≠")]
    public int maxHealth;
    public float shrink = 0.8f;
    public float duration = 0.5f;
    private int currentHealth;

    void Start()
    {
        if (circleCollider != null)
            radius = circleCollider.radius * Mathf.Max(transform.localScale.x, transform.localScale.y);
    }

    public void GetAttack()
    {
        currentHealth--;

        if(currentHealth <= 0)
        {
            currentHealth = 0;
        }

        Vector3 shrinkScale = transform.localScale * shrink;
        transform.DOScale(shrinkScale, duration).SetEase(Ease.OutBounce).OnComplete(UpdateRadius);
    }

    private void UpdateRadius()
    {
        radius = circleCollider.radius * Mathf.Max(transform.localScale.x, transform.localScale.y);
    }

    public float ReturnRadius()
    {
        return radius;
    }
}
