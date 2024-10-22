using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeathBringerCastState : EnemyState
{
    private Enemy_DeathBringer enemy;

    private int amountOfCasts;
    private float castTimer;

    public DeathBringerCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        amountOfCasts = enemy.amountOfCasts;
        castTimer = .5f;
    }

    public override void Update()
    {
        base.Update();
        
        castTimer -= Time.deltaTime;

        if (CanCast())
        {
            amountOfCasts--;
            enemy.Cast();
        }
        
        if (amountOfCasts <= 0)
        {
            stateMachine.changeState(enemy.teleportState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeCast = Time.time;
    }

    private bool CanCast()
    {
        if (amountOfCasts > 0 && castTimer < 0)
        {
            castTimer = enemy.castCooldown;
            return true;
        }

        return false;
    }
}
