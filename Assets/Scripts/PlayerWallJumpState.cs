using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    // Start is called before the first frame update
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = .4f;
        player.SetVelocity(5 * -player.facingDir,player.jumpForce);
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer<0)
        {
            stateMachine.changeState(player.airState);
        }

        if (player.IsGroundDetected())
        {
            stateMachine.changeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
