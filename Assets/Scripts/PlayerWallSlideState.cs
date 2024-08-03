using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    protected float wallSlideVelocityFactor = 0.92f;

    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.changeState(player.wallJumpState);
            return;
        }

        if (xInput != 0 && player.facingDir != xInput)
        {
            stateMachine.changeState(player.idleState);
            return;
        }

        if (yInput < 0)
        {
            player.setVelocity(0, rb.velocity.y);
        }
        else
        {
            player.setVelocity(0, rb.velocity.y * wallSlideVelocityFactor);
        }

        if (player.IsGoundDetected())
        {
            stateMachine.changeState(player.idleState);
            return;
        }
    }
}
