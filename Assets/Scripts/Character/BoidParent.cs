using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidParent : MonoBehaviour
{
    public float boidAttackInterval = 1f;
    [SerializeField] private List<LightingBoid> boids = new List<LightingBoid>();
    private Transform enemyTransform;

    public void GetBoid(LightingBoid newBoid)
    {
        boids.Add(newBoid);
        foreach (var boid in boids)
            boid.UpdateBoidsList(boids);
    }

    public void InvokeBoidAttack()
    {
        if (enemyTransform = GameObject.FindWithTag("BlackBoid").transform)
            StartCoroutine(BoidsAttack());
    }

    private IEnumerator BoidsAttack()
    {
        for (int i = 0; i < boids.Count; i++)
        {
            boids[i].AttackStart(enemyTransform);
            yield return new WaitForSeconds(boidAttackInterval);
        }
    }

    private void Start()
    {
        StartCoroutine(attack());
    }

    private IEnumerator attack()
    {
        yield return new WaitForSeconds(10f);
        InvokeBoidAttack();
    }
}
