using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfArcherJumpState : EnemyState
{
    private Enemy_ElfArcher enemy;

    public ElfArcherJumpState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_ElfArcher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(enemy.jumpVelocity.x * -enemy.facingDir, enemy.jumpVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        enemy.anim.SetFloat("yVelocity", rb.velocity.y);

        if (rb.velocity.y < 0 && enemy.IsGroundDetected())
        {
            stateMachine.changeState(enemy.battleState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
