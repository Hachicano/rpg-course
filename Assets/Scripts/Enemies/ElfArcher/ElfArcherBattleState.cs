using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfArcherBattleState : EnemyState
{
    protected Enemy_ElfArcher enemy;
    private Transform player;
    private int moveDir;

    private float giveupDistance = 7;

    public ElfArcherBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_ElfArcher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
            if (enemy.IsPlayerDetected().distance <= enemy.safeDistance)
            {
                if (canJump())
                {
                    stateMachine.changeState(enemy.jumpState);
                    return;
                }
            }

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

        BattleStateFlipControll();
    }

    private void BattleStateFlipControll()
    {
        if (player.position.x > enemy.transform.position.x && enemy.facingDir == -1)
            enemy.Flip();
        else if (player.position.x < enemy.transform.position.x && enemy.facingDir == 1)
            enemy.Flip();
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

    private bool canJump()
    {
        if (enemy.GroundBehind() == false || enemy.WallBehind() == true)
            return false;

        if (Time.time >= enemy.lastTimeJumped + enemy.jumpCooldown)
        {
            enemy.lastTimeJumped = Time.time;
            return true;
        }

        return false;
    }
}
