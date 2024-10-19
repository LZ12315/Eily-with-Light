using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCtrl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;
    public float curSpeed;
    public bool isGround;
    public bool isBouncing;

    /*�ֶ�����*/
    public float speed=5.8f, jumpForce=9f, acceleration=1.5f;
    public Transform groundCheck;
    public LayerMask ground;


    /*�������ר��*/
    private bool isRaising;
    private bool isFalling;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        curSpeed = 0;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump")&&isGround/*&&jumpCount>0*/)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);//��Ծ
        }
        if(Input.GetKeyDown(KeyCode.S)&&!isGround)
        {
            rb.gravityScale = 10;//��������
        }
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, ground);//��ؼ��
        float moveDir = Input.GetAxisRaw("Horizontal"); // ��ȡ�ƶ�����
        GroundMovement(moveDir);
        AirMovement(moveDir);
        AnimPlay();
    }

    void GroundMovement(float moveDir)
    {
        if (isGround)
        {
            isBouncing = false;
            rb.gravityScale = 1;
            // �������뷽����ٻ����
            if (moveDir != 0)
            {
                curSpeed = Mathf.MoveTowards(curSpeed, speed, acceleration * Time.fixedDeltaTime);
                transform.localScale = new Vector3(moveDir, 1, 1); // ���ҷ�ת
            }
            else
            {
                curSpeed = (Mathf.MoveTowards(curSpeed, 0, acceleration * Time.fixedDeltaTime));
            }
            rb.velocity = new Vector2(transform.localScale.x * curSpeed, rb.velocity.y); // �ƶ�
        }
    }

    void AirMovement(float moveDir)//���п���ɲ��������ת��
    {
        if(!isGround&& moveDir != 0&&!isBouncing)
        {
            if (Mathf.Sign(transform.localScale.x)!=moveDir)
            {
                curSpeed = Mathf.MoveTowards(curSpeed, 0, acceleration * Time.fixedDeltaTime);
            }
            else
            {
                curSpeed = Mathf.MoveTowards(curSpeed, speed, acceleration * Time.fixedDeltaTime);
            }
            rb.velocity = new Vector2(transform.localScale.x*curSpeed, rb.velocity.y); // �ƶ�
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(!isGround&& collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            isBouncing = true;
        }
    }

    void AnimPlay()
    {
        
    }
}
