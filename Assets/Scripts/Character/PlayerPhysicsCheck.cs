using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsCheck : MonoBehaviour
{
    [Header("µØÃæ¼ì²é")]
    public bool isGround;
    public LayerMask groundLayer;
    public Vector2 checkOffset;
    public float checkRadius;
    public float coyoteTime;
    private float coyoteCounter;

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void GroundCheck()
    {
        isGround = Physics2D.OverlapCircle(transform.position + (Vector3)checkOffset, checkRadius, groundLayer);

        if (isGround)
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.fixedDeltaTime;

            if (coyoteCounter > 0)
            {
                isGround = true;
            }
            else
            {
                isGround = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)checkOffset, checkRadius);
    }
}
