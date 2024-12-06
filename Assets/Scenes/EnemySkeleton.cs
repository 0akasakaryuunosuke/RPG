using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Entity
{
    [SerializeField] private bool isAttacking;
    [Header("移动参数")]
    [SerializeField] private float moveSpeed;

    [Header("玩家检测参数")] 
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask whatIsPlayer;
    private RaycastHit2D isPlayerDetected;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (isPlayerDetected)
        {
            if (isPlayerDetected.distance > 2)
            {
                rb.velocity = new Vector2(moveSpeed*facingDir*1.5f, rb.velocity.y);
                isAttacking = false;
            }
            else
            {
                isAttacking = true;
            }
        }
        Movement();
        if(!isGrounded||isWallDetected)
        {
            Flip();
        }
    }

    private void Movement()
    {
        if(!isAttacking)
        {
            rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
        }
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();
        isPlayerDetected = Physics2D.Raycast(transform.position
                , Vector2.right
                , playerCheckDistance * facingDir
                , whatIsPlayer);
        
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color=Color.cyan;
        Gizmos.DrawLine(transform.position,new Vector3(transform.position.x+playerCheckDistance*facingDir,transform.position.y));
    }
}
