using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Test : MonoBehaviour
{
    public Rigidbody2D rb;

    public float speed;
    public int direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            direction = 1;
        else if(Input.GetMouseButton(1))
            direction = -1;
        else
            direction = 0;

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        if(direction != 0)
            transform.localScale = new Vector3(-direction, 1, 1);
    }
}
