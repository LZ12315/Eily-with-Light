using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidParent : MonoBehaviour
{
    [SerializeField] private List<LightingBoid> boids = new List<LightingBoid>();

    public void GetBoid(LightingBoid newBoid)
    {
        boids.Add(newBoid);
        foreach (var boid in boids)
            boid.UpdateBoidsList(boids);
    }

    public void BoidAttack()
    {

    }
}
