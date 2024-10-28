using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidParent : MonoBehaviour
{
    public float boidAttackInterval = 1f;
    [SerializeField] public List<LightingBoid> boids = new List<LightingBoid>();
    private List<LightingBoid> boidsToRemove = new List<LightingBoid>();
    private Transform enemyTransform;

    public void GetBoid(LightingBoid newBoid)
    {
        boids.Add(newBoid);
        foreach (var boid in boids)
            boid.UpdateBoidsList(boids);
    }

    public void DeleteBoid(LightingBoid boidToDelete)
    {
        if (boids.Contains(boidToDelete))
            boids.Remove(boidToDelete);
    }

    public void InvokeBoidAttack()
    {
        if (enemyTransform = GameObject.FindWithTag("BlackBoid").transform)
            StartCoroutine(BoidsAttack());
    }

    private IEnumerator BoidsAttack()
    {
        for (int i = boids.Count - 1; i >= 0; i--)
        {
            boids[i].StartAttack(enemyTransform);
            yield return new WaitForSeconds(boidAttackInterval);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
            InvokeBoidAttack();
    }
}
