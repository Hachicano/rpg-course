using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ElfArcher : Enemy
{
    #region State
    public ElfArcherIdleState idleState { get; private set; }
    public ElfArcherMoveState moveState { get; private set; }
    public ElfArcherBattleState battleState { get; private set; }
    public ElfArcherAttackState attackState { get; private set; }
    public ElfArcherStunnedState stunnedState { get; private set; }
    public ElfArcherDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        idleState = new ElfArcherIdleState(this, stateMachine, "Idle", this);
        moveState = new ElfArcherMoveState(this, stateMachine, "Move", this);
        battleState = new ElfArcherBattleState(this, stateMachine, "Move", this);
        attackState = new ElfArcherAttackState(this, stateMachine, "Attack", this);
        stunnedState = new ElfArcherStunnedState(this, stateMachine, "Stunned", this);
        deadState = new ElfArcherDeadState(this, stateMachine, "Dead", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.changeState(deadState);
    }
}
