using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharCtrl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;
    private float curSpeed;
    public bool isGround;
    private bool isBouncing;
    private float curScale = 0f;
    private TimelineManager timelineManager;

    /*�ֶ�����*/
    public float speed=5.8f, jumpForce=9f, acceleration=2.5f;
    public Transform groundCheck;
    public LayerMask ground;
    public ParticleSystem particle;
    public GameObject camFollowPos;

    /*�������ר��*/
    private bool isRaising;
    private bool isFalling;

    void Awake()
    {
        timelineManager = GetComponent<TimelineManager>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        curSpeed = 0;
    }

    void Update()
    {
        if(!timelineManager.isPlaying)
        {
            if (Input.GetButtonDown("Jump") && isGround/*&&jumpCount>0*/)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);//��Ծ
            }
            if (Input.GetKeyDown(KeyCode.S) && !isGround)
            {
                rb.gravityScale = 10;//��������
            }
            isGround = Physics2D.OverlapCircle(groundCheck.position, 0.5f, ground);//��ؼ��
            float moveDir = Input.GetAxisRaw("Horizontal"); // ��ȡ�ƶ�����
            GroundMovement(moveDir);
            AirMovement(moveDir);
            AnimPlay();
            ParticlePlay();
            SetCamPos();
        }
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
        //if (isGround && rb.velocity.x == 0)
        //{
        //    Debug.Log("Idle");
        //}
        //else if (isGround && rb.velocity.x != 0)
        //{
        //    Debug.Log("Running");
        //}
        //else if (!isGround && rb.velocity.y >= 0)
        //{
        //    Debug.Log("Raising");
        //}
        //else if ((!isGround && rb.velocity.y < 0))
        //{
        //    Debug.Log("Falling");
        //}
    }

    void ParticlePlay()
    {
        //�������ӷ���
        float angle = Mathf.Atan2(rb.velocity.x, -rb.velocity.y) * Mathf.Rad2Deg;

        if (rb.velocity!=Vector2.zero)
        {
            //��������
            particle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            curScale = (Mathf.MoveTowards(curScale,1f, 0.3f * Time.fixedDeltaTime));
        }
        else
        {
            particle.transform.rotation = Quaternion.Euler(new Vector3(90,0, angle));
            curScale = (Mathf.MoveTowards(curScale, 0f, 0.3f * Time.fixedDeltaTime));
        }
        particle.transform.localScale = new Vector3(curScale, curScale, curScale);
    }

    void SetCamPos()
    {
            //camFollowPos.transform.position = new Vector3(transform.position.x + 4.5f, transform.position.y, transform.position.z);
    }
}
