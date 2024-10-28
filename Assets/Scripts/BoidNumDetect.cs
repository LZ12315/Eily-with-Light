using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidNumDetect : MonoBehaviour
{
    public BoidParent parent;
    public int num=4;
    private Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (parent.boids.Count >= num)
        {
            collider.enabled = true;
        }
        else 
        { 
            collider.enabled = false; 
        }
    }
}
