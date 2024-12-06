using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    // Start is called before the first frame update
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.dashDuration;
        
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(player.dashSpeed*player.dashDir,0);
        if (stateTimer < 0)
        {
            stateMachine.changeState(player.idleState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(player.moveSpeed,rb.velocity.y);
    }
}
