using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("类")]
    public PlayerInputSystem inputControl;
    private Rigidbody2D rb;
    private PlayerPhysicsCheck physicsCheck;
    public Vector2 inputAction;
    public Vector2 lastInputAction = new Vector2(1, 0);

    [Header("角色状态")]
    public bool isRun;

    [Header("移动参数")]
    public bool canMove = true;
    public float currentSpeed;
    public float normalSpeed;
    public float accelerationTime;
    public float decelerationTime;
    private float timeElapsed_ac;
    private float timeElapsed_dc;

    [Header("跳跃参数")]
    public float jumpForce;
    public float floatFreezeCoefficient;
    public int floatJumpLimit;
    private int floatJumpCounter;

    public float jumpPreTime;
    private float jumpPreCounter;
    private bool jumpPreInput;

    private void Awake()
    {
        inputControl = new PlayerInputSystem();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PlayerPhysicsCheck>();
    }

    private void OnEnable()
    {
        inputControl.Enable();
        inputControl.Player.Jump.started += JumpInvoke;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        inputControl.Player.Jump.started -= JumpInvoke;
    }

    private void Update()
    {
        inputAction = inputControl.Player.Move.ReadValue<Vector2>();

        CheckPreJump();
        JumpTimerReload();
    }

    private void FixedUpdate()
    {
        TrunOver();
        Move();
    }

    private void TrunOver()
    {
        if(inputAction.x > 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if(inputAction.x < 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void Move()
    {
        if (canMove)
        {
            Walk();

            if (physicsCheck.isGround)
                rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
            else
                rb.velocity = new Vector2(currentSpeed * floatFreezeCoefficient, rb.velocity.y);
        }
    }

    private void Walk()
    {
        if (Mathf.Abs(inputAction.x) > 0)
        {
            timeElapsed_dc = 0;
            if (currentSpeed < normalSpeed)
            {
                timeElapsed_ac += Time.fixedDeltaTime;
                float t = timeElapsed_ac / accelerationTime;
                currentSpeed = Mathf.Lerp(currentSpeed, normalSpeed * Mathf.Sign(inputAction.x), t);
            }
            else
            {
                currentSpeed = normalSpeed * Mathf.Sign(inputAction.x);
            }
            lastInputAction = inputAction;
        }
        else if (Mathf.Abs(inputAction.x) == 0)
        {
            if (timeElapsed_dc < decelerationTime)
            {
                timeElapsed_dc += Time.fixedDeltaTime;
                float t = timeElapsed_dc / decelerationTime;
                currentSpeed = Mathf.Lerp(currentSpeed, 0, t);

                if (timeElapsed_dc > decelerationTime / 3)
                    timeElapsed_ac = 0;
            }
            else
            {
                currentSpeed = 0;
                timeElapsed_dc = 0;
            }
        }
    }

    private void JumpInvoke(InputAction.CallbackContext context)
    {
        Jump();
    }

    private void Jump()
    {
        if (physicsCheck.isGround || floatJumpCounter < floatJumpLimit)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            floatJumpCounter++;
            jumpPreInput = false;
        }
        else
        {
            jumpPreInput = true;
        }
    }

    private void JumpTimerReload()
    {
        if (physicsCheck.isGround)
        {
            floatJumpCounter = 0;
        }
    }

    private void CheckPreJump()
    {
        if (jumpPreInput)
        {
            jumpPreCounter += Time.deltaTime;
            if (jumpPreCounter >= jumpPreTime)
            {
                jumpPreCounter = 0;
                jumpPreInput = false;
            }
            else
            {
                Jump();
            }
        }
    }
}
