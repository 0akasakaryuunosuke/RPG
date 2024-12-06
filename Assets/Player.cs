using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
   
   public int facingDir{ get; private set; } = 1;
   private bool facingRight = true;
   [Header("移动参数")]
   public float moveSpeed ;
   public float jumpForce ;
   [Header("冲刺参数")] 
   [SerializeField] private float dashCooldown;
   private float dashCooldownTimer;
   public float dashSpeed;
   public float dashDuration;
   public float dashDir { get; private set; }
   [Header("碰撞参数")] 
   [SerializeField] protected Transform groundCheck;
   [SerializeField] protected float groundCheckDistance;
   [SerializeField] protected LayerMask whatIsGround;
   [SerializeField] protected Transform wallCheck;
   [SerializeField] protected float wallCheckDistance;
   //[SerializeField] protected LayerMask whatIsWall;
   public Rigidbody2D rb { get; private set; }
   public Animator anim { get; private set; }
   public PlayerStateMachine stateMachine { get; private set; }
   public PlayerIdleState idleState{ get; private set; }
   public PlayerMoveState moveState{ get; private set; }
   public PlayerJumpState jumpState{ get; private set; }
   public PlayerAirState  airState { get; private set; }
   public PlayerDashState  dashState { get; private set; }
   public PlayerWallSlideState  wallSlideState { get; private set; }
   private void Awake()
   {
      stateMachine = new PlayerStateMachine();
      idleState = new PlayerIdleState(this,stateMachine,"Idle");
      moveState = new PlayerMoveState(this, stateMachine, "Move");
      jumpState = new PlayerJumpState(this, stateMachine, "Jump");
      airState = new PlayerAirState(this, stateMachine, "Jump");
      dashState = new PlayerDashState(this, stateMachine, "Dash");
      wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
   }

   private void Start()
   {
      rb = GetComponent<Rigidbody2D>();
      anim = GetComponentInChildren<Animator>();
      stateMachine.Initialize(idleState);
      if (wallCheck == null) wallCheck = transform;
   }

   private void Update()
   {
      stateMachine.currentState.Update();
      CheckDashInput();
   }

   private void CheckDashInput()
   {
      dashCooldownTimer -= Time.deltaTime;
      if (Input.GetKeyDown(KeyCode.LeftShift)&&dashCooldownTimer<0)
      {
         dashCooldownTimer = dashCooldown;
         dashDir = Input.GetAxisRaw("Horizontal")==0?Input.GetAxisRaw("Horizontal"):facingDir;
         stateMachine.changeState(dashState);
      }
   }

   public void SetVelocity(float xVelocity, float yVelocity)
   {
      FlipController(xVelocity);
      rb.velocity = new Vector2(xVelocity, yVelocity);
   }

   public void Flip()
   {
      facingDir = facingDir * -1;
      facingRight = !facingRight;
      transform.Rotate(0,180,0);
   }

   public void FlipController(float _x)
   {
      if (_x >0 && !facingRight)
      {
        Flip();
      }
      else if(_x < 0 && facingRight)
      {
        Flip();
      }
   }
   public bool IsGroundDetected() =>
      Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
   public bool IsWallDetected() =>
      Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance*facingDir, whatIsGround);
   protected virtual void OnDrawGizmos()
   {
      Gizmos.DrawLine(groundCheck.position,new Vector3(groundCheck.position.x,groundCheck.position.y-groundCheckDistance));
      Gizmos.DrawLine(wallCheck.position,new Vector3(wallCheck.position.x + wallCheckDistance*facingDir,wallCheck.position.y) );
   }
}
