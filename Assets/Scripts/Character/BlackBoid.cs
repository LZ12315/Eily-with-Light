using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoid : MonoBehaviour
{
    [Header(" ‹…À¬ﬂº≠")]
    public int maxHealth;
    public float shrink = 0.8f;
    public float duration = 0.5f;
    private int currentHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<LightingBoid>() != null)
            GetAttack();
    }

    private void GetAttack()
    {
        currentHealth--;

        if(currentHealth <= 0)
        {
            currentHealth = 0;
        }

        Vector3 shrinkScale = transform.localScale * shrink;
        transform.DOScale(shrinkScale, duration).SetEase(Ease.OutBounce);
    }
}
