using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void SetupTransitions()
    {
        base.SetupTransitions();
        this.transitions.Add(new Transition(player.idleState, () => stateTimer < 0));
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.dash.CreateCloneOnDash();
        stateTimer = player.dashDuration;
        player.stats.MakeInvincible(true);
    }

    public override void Exit()
    {
        base.Exit();
        player.setVelocity(0, rb.velocity.y);
        player.skill.dash.CreateCloneOnDashArrival();
        player.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();
        player.fx.CreateAfterImage();
        if (!player.IsGroundDetected() && player.IsWallDetected())
        {
            stateMachine.changeState(player.wallSlideState);
            return;
        }

        player.setVelocity(player.dashSpeed * player.dashDir, 0);

        /*
        if (stateTimer < 0)
        {
            stateMachine.changeState(player.idleState);
            return;
        }
        */

    }
}
