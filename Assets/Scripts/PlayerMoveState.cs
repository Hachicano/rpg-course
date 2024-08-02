using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string __animBoolName) : base(_player, _stateMachine, __animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        player.setVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (xInput == 0)
        {
            player.stateMachine.changeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
