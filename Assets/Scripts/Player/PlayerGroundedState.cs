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

        if (Input.GetKeyDown(KeyCode.T) && player.skill.blackhole.blackholeUnlocked && player.skill.blackhole.GetCooldownTimer() <= 0)
        {
            stateMachine.changeState(player.blackholeState);
            return;
        }
        else if(Input.GetKeyDown(KeyCode.T) && player.skill.blackhole.blackholeUnlocked && player.skill.blackhole.GetCooldownTimer() > 0)
            player.fx.CreatePopUpText("Blackhole is on cooldown");

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.sword.swordUnlocked)
        {
            stateMachine.changeState(player.aimSword);
            return;
        }

        if(Input.GetKeyDown(KeyCode.F) && player.skill.parry.parryUnlocked && player.skill.parry.CanUseSkill())
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

    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }
        player.sword.GetComponent<Skill_Sword_Controller>().ReturnSword();
        return false;
    }
}
