using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    // Start is called before the first frame update
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        rb.velocity = yInput<0 ? new Vector2(0, rb.velocity.y):new Vector2(0, rb.velocity.y * .4f);
        if (player.IsGroundDetected())
        {
            stateMachine.changeState(player.idleState);
        }
        if (xInput != 0 && player.facingDir != xInput)
        {
            stateMachine.changeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
