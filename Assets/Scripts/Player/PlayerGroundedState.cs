using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string __animBoolName) : base(_player, _stateMachine, __animBoolName)
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

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            stateMachine.changeState(player.counterAttack);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.changeState(player.primaryAttack);
            return;
        }

        if (!player.IsGoundDetected()) 
        {
            stateMachine.changeState(player.airborneState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGoundDetected()) {
            stateMachine.changeState(player.jumpState);
            return;
        }
    }
}
