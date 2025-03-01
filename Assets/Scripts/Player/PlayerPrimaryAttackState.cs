using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter {  get; private set; }

    public float lastTimeAttack { get; private set; }
    public float attackDir;
    private float comboWindow = 2;
    private float inertiaWindow = 0.12f;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void SetupTransitions()
    {
        base.SetupTransitions();
        this.transitions.Add(new Transition(player.idleState, () => triggerCalled));
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0;
        comboCounter = comboCounter % 3;
        AudioManager.instance.PlayerSFX(comboCounter, null); // null means this distance check wont work           sfx_attack1-3
        if (Time.time >= lastTimeAttack + comboWindow)
        {
            comboCounter = 0;
        }
        player.anim.SetInteger("comboCounter", comboCounter);
        // player.anim.speed = 1.2f; // 修改攻速

        if (xInput != 0)
        {
            attackDir = xInput;
        }
        else
        {
            attackDir = player.facingDir;
        }

        player.setVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = inertiaWindow;
    }

    public override void Exit()
    {
        base.Exit();
        // player.StartCoroutine(nameof(player.BusyFor), .05f); // 手感不好

        // player.anim.speed = 1; 修改攻速
        comboCounter++;
        // Debug.Log(comboCounter.ToString());
        lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            player.setZeroVelocity();
        }
        /*
        if (triggerCalled)
        {
            stateMachine.changeState(player.idleState);
            return;
        }*/
    }
}
