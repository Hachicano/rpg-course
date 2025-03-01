using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string __animBoolName) : base(_player, _stateMachine, __animBoolName)
    {
    }

    public override void SetupTransitions()
    {
        base.SetupTransitions();
        this.transitions.Add(new Transition(player.blackholeState, () => Input.GetKeyDown(KeyCode.T) && player.skill.blackhole.blackholeUnlocked && player.skill.blackhole.GetCooldownTimer() <= 0));
        this.transitions.Add(new Transition(player.aimSword, () => Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.sword.swordUnlocked));
        this.transitions.Add(new Transition(player.counterAttack, () => Input.GetKeyDown(KeyCode.F) && player.skill.parry.parryUnlocked && player.skill.parry.CanUseSkill()));
        this.transitions.Add(new Transition(player.primaryAttack, () => Input.GetKeyDown(KeyCode.Mouse0)));
        this.transitions.Add(new Transition(player.airborneState, () => !player.IsGroundDetected()));
        this.transitions.Add(new Transition(player.jumpState, () => Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected()));
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
        if (Input.GetKeyDown(KeyCode.T) && player.skill.blackhole.blackholeUnlocked && player.skill.blackhole.GetCooldownTimer() > 0)
            player.fx.CreatePopUpText("Blackhole is on cooldown");
        /*
        if (Input.GetKeyDown(KeyCode.T) && player.skill.blackhole.blackholeUnlocked && player.skill.blackhole.GetCooldownTimer() <= 0)
        {
            stateMachine.changeState(player.blackholeState);
            return;
        }
 
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

        if (!player.IsGroundDetected()) 
        {
            stateMachine.changeState(player.airborneState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected()) {
            stateMachine.changeState(player.jumpState);
            return;
        }
        */
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
