using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfArcherGroundedState : EnemyState
{
    protected Enemy_ElfArcher enemy;
    protected Transform player;

    public ElfArcherGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_ElfArcher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < enemy.angerDistance)
        {
            stateMachine.changeState(enemy.battleState);
            return;
        }
    }
}
