using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    private CharCtrl CharCtrl;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        CharCtrl = GetComponent<CharCtrl>();
    }

    private void Update()
    {
        animator.SetFloat("VelocityX",Mathf.Abs(rb.velocity.x));
        animator.SetFloat("VelocityY",rb.velocity.y);
        animator.SetBool("isGround",CharCtrl.isGround);
    }
}
