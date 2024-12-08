using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    // Start is called before the first frame update
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (player.IsGroundDetected())
        {
            stateMachine.changeState(player.idleState);
        }

        if (player.IsWallDetected())
        {
            stateMachine.changeState(player.wallSlideState);
        }
        if (xInput != 0)
        {
            player.SetVelocity(player.moveSpeed * xInput * .8f,rb.velocity.y);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
    }
}