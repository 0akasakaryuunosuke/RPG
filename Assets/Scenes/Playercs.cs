using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercs : Entity
{
    private float xInput;
    private float yInput;
    [Header("人物参数")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float movingSpeed;
    [Header("冲刺参数")] 
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;
     private float dashTime;
     private float dashCooldownTimer;
    [Header("攻击参数")]
    [SerializeField] private float comboTime;
    [SerializeField] private float comboTimeCounter;
    private bool isAttcack;
    private int comboAttack;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        dashSpeed = 15;
        jumpForce = 5;
        movingSpeed = 5;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Movement();
        CheckInput();
        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        comboTimeCounter -= Time.deltaTime;
        FlipController();
        AnimatorController();
    }

    public void AttackOver()
    {
        isAttcack = false;
        comboAttack++;
        if (comboAttack > 2) comboAttack = 0;
    }
   

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal"); 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (!isGrounded) return;
        isAttcack = true;
        if (comboTimeCounter < 0)
        {
            comboAttack = 0;
        }
        comboTimeCounter = comboTime;
    }

    private void Dash()
    {
        if(dashCooldownTimer<0&& !isAttcack)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }

    private void Movement()
    {
        if (isAttcack && isGrounded)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else if (dashTime > 0)
        {
            rb.velocity = new Vector2(xInput * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * movingSpeed, rb.velocity.y);
        }
    }

    private void Jump()
    {
        if(isGrounded)
          rb.velocity = new Vector2(xInput, jumpForce);
    }

   

    private void AnimatorController()
    {
        bool isMoving = rb.velocity.x != 0;
        anm.SetFloat("yVelocity",rb.velocity.y);
        anm.SetBool("IsMoving", isMoving);
        anm.SetBool("IsGrounded",isGrounded);
        anm.SetBool("IsDashing", dashTime>0);
        anm.SetBool("IsAttack", isAttcack);
        anm.SetInteger("ComboAttack", comboAttack);
    }
    
    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight) Flip();
        else if (rb.velocity.x < 0 && facingRight) Flip();
    }
    
}
