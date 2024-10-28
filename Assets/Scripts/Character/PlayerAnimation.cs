using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public PlayerPhysicsCheck PlayerPhysicsCheck;
    private CharCtrl CharCtrl;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        PlayerPhysicsCheck = GetComponent<PlayerPhysicsCheck>();
        CharCtrl = GetComponent<CharCtrl>();
    }

    private void Update()
    {
        animator.SetFloat("VelocityX",Mathf.Abs(rb.velocity.x));
        animator.SetFloat("VelocityY",rb.velocity.y);
        animator.SetBool("isGround",PlayerPhysicsCheck.isGround);
        //animator.SetBool("isGround",CharCtrl.isGround);
    }
}
