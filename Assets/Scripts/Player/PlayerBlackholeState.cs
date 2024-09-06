using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = .4f;
    private bool skilUsed;
    public bool skillFinished;
    private float defaultGravity;

    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity = rb.gravityScale;
        skillFinished = false;
        skilUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravity;
        player.fx.MakeTransparent(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0) 
        {
            rb.velocity = new Vector2(0, 12);
        }
        else if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -.1f);
            if (!skilUsed)
            {
                if(player.skill.blackhole.CanUseSkill())
                    skilUsed = true;
            }
        }
        
        if (player.skill.blackhole.BlackholeSkillFinish())
        {
            skillFinished = true;
            stateMachine.changeState(player.airborneState);
            return;
        }

    }
}
