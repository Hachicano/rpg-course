using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
    private Enemy_DeathBringer enemy;
    protected Transform player;

    public DeathBringerIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0 && enemy.bossFightBegun)
        {
            stateMachine.changeState(enemy.battleState);
            return;
        }

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < enemy.angerDistance)
        {
            stateMachine.changeState(enemy.battleState);
            enemy.bossFightBegun = true;
            return;
        }

    }
}
