using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerBattleState : EnemyState
{
    private Enemy_DeathBringer enemy;
    private Transform player;
    private int moveDir;

    private float giveupDistance = 20;

    public DeathBringerBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.battleTime;
        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.changeState(enemy.idleState);
            return;
        }
    }


    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (canAttack())
                {
                    stateMachine.changeState(enemy.attackState);
                    return;
                }
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > giveupDistance)
            {
                stateMachine.changeState(enemy.idleState);
                return;
            }
        }

        if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }

        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance - .1f)
            return;

        enemy.setVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }
    public override void Exit()
    {
        base.Exit();
    }

    private bool canAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            return true;
        }

        return false;
    }
}
