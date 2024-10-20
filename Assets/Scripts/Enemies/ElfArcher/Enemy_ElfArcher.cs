using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ElfArcher : Enemy
{
    [Header("ElfArcher Specific")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed;

    [HideInInspector] public float lastTimeJumped;
    public Vector2 jumpVelocity;
    public float jumpCooldown;
    public float safeDistance;  // how close player should to trigger jump on battle state

    [Header("Additional Collision Check")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;

    #region State
    public ElfArcherIdleState idleState { get; private set; }
    public ElfArcherMoveState moveState { get; private set; }
    public ElfArcherBattleState battleState { get; private set; }
    public ElfArcherAttackState attackState { get; private set; }
    public ElfArcherStunnedState stunnedState { get; private set; }
    public ElfArcherDeadState deadState { get; private set; }
    public ElfArcherJumpState jumpState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        idleState = new ElfArcherIdleState(this, stateMachine, "Idle", this);
        moveState = new ElfArcherMoveState(this, stateMachine, "Move", this);
        battleState = new ElfArcherBattleState(this, stateMachine, "Idle", this);
        attackState = new ElfArcherAttackState(this, stateMachine, "Attack", this);
        stunnedState = new ElfArcherStunnedState(this, stateMachine, "Stunned", this);
        deadState = new ElfArcherDeadState(this, stateMachine, "Move", this);
        jumpState = new ElfArcherJumpState(this, stateMachine, "Jump", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.changeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.changeState(deadState);
    }

    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newArrow = Instantiate(arrowPrefab, attackCheck.position, Quaternion.identity);
        newArrow.GetComponent<ArrowController>().SetUpArrow(stats, arrowSpeed * facingDir);
        if (facingDir == -1)
            newArrow.transform.Rotate(0, 180, 0);
    }

    public bool GroundBehind() => Physics2D.BoxCast(groundBehindCheck.position, groundBehindCheckSize, 0, Vector2.zero, whatIsGround);
    public bool WallBehind() => Physics2D.Raycast(wallCheck.position, Vector2.right * -facingDir, wallCheckDistance + 2, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireCube(groundBehindCheck.position, groundBehindCheckSize);
    }
}
