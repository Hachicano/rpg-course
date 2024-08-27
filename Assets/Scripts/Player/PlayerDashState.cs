using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{

    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.dash.CreateCloneOnDash();
        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();
        player.setVelocity(0, rb.velocity.y);
        player.skill.dash.CreateCloneOnDashArrival();
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGoundDetected() && player.IsWallDetected())
        {
            stateMachine.changeState(player.wallSlideState);
            return;
        }

        player.setVelocity(player.dashSpeed * player.dashDir, 0);

        if (stateTimer < 0)
        {
            stateMachine.changeState(player.idleState);
            return;
            /*
            if (stateMachine.oldState == player.jumpState)
            {
                stateMachine.changeState(player.airborneState);
                return;
            }
            else
            {
                stateMachine.changeState(stateMachine.oldState);
                return;
            }
            */
        }
    }
}
