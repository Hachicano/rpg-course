using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfArcherDeadState : EnemyState
{
    protected Enemy_ElfArcher enemy;

    public ElfArcherDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_ElfArcher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;
        // enemy.getCounterImage().SetActive(false);
        // = .15f;
    }

    public override void Update()
    {
        base.Update();
        /*
        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 8);
        }
        */
    }

    public override void Exit()
    {
        base.Exit();
        enemy.sr.color = new Color(enemy.sr.color.r, enemy.sr.color.g, enemy.sr.color.b, 0);
        enemy.GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
