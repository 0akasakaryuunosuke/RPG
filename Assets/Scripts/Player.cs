using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
   public bool isBusy { get; private set; }
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

   [Header("攻击参数")] 
   [SerializeField] public Vector2[] attackMovement;
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
   public PlayerWallJumpState  wallJumpState  { get; private set; }
   
   public PlayerPrimaryAttackState primaryAttackState { get; private set; }
   private void Awake()
   {
      stateMachine = new PlayerStateMachine();
      idleState = new PlayerIdleState(this,stateMachine,"Idle");
      moveState = new PlayerMoveState(this, stateMachine, "Move");
      jumpState = new PlayerJumpState(this, stateMachine, "Jump");
      airState = new PlayerAirState(this, stateMachine, "Jump");
      dashState = new PlayerDashState(this, stateMachine, "Dash");
      wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
      wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
      primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
   }

   private void Start()
   {
      rb = GetComponent<Rigidbody2D>();
      anim = GetComponentInChildren<Animator>();
      stateMachine.Initialize(idleState);
     // if (wallCheck == null) wallCheck = transform;
   }

   private void Update()
   {
      stateMachine.currentState.Update();
      CheckDashInput();
      
   }

   public IEnumerator BusyFor(float _seconds)
   {
      isBusy = true;
      yield return new WaitForSeconds(_seconds);
      isBusy = false;

   }

   public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
   private void CheckDashInput()
   {
      if (IsWallDetected()) return;
      dashCooldownTimer -= Time.deltaTime;
      if (Input.GetKeyDown(KeyCode.LeftShift)&&dashCooldownTimer<0)
      {
         dashCooldownTimer = dashCooldown;
         dashDir = Input.GetAxisRaw("Horizontal")!=0?Input.GetAxisRaw("Horizontal"):facingDir;
         stateMachine.changeState(dashState);
      }
   }

   #region 速度控制
   public void ZeroVelocity() => SetVelocity(0, 0);
   public void SetVelocity(float xVelocity, float yVelocity)
   {
      FlipController(xVelocity);
      rb.velocity = new Vector2(xVelocity, yVelocity);
   }
   
   #endregion
   #region 翻转控制

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
   
   #endregion
   #region 碰撞相关
   public bool IsGroundDetected() =>
      Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
   public bool IsWallDetected() =>
      Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance*facingDir, whatIsGround);
   protected virtual void OnDrawGizmos()
   {
      Gizmos.DrawLine(groundCheck.position,new Vector3(groundCheck.position.x,groundCheck.position.y-groundCheckDistance));
      Gizmos.DrawLine(wallCheck.position,new Vector3(wallCheck.position.x + wallCheckDistance*facingDir,wallCheck.position.y) );
   }
   #endregion
}
