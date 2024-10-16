using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBattleState : EnemyState
{
    private Transform player;
    protected Enemy_Slime enemy;
    private int moveDir;

    private float giveupDistance = 7;

    public SlimeBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.battleTime;
        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.changeState(enemy.moveState);
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
