using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneState : PlayerState
{
    public PlayerAirborneState(Player _player, PlayerStateMachine _stateMachine, string __animBoolName) : base(_player, _stateMachine, __animBoolName)
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
        player.setVelocity(xInput * player.moveSpeed, rb.velocity.y);
        if (player.IsGoundDetected())
        {
            stateMachine.changeState(player.idleState);
        }
    }
}
