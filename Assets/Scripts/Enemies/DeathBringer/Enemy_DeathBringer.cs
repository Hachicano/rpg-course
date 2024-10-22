using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy_DeathBringer : Enemy
{
    public bool bossFightBegun;

    [Header("Cast Details")]
    [SerializeField] private GameObject castPrefab;
    [SerializeField] private float castStateCooldown;
    [SerializeField] private Vector2 castOffset;
    public int amountOfCasts;
    public float castCooldown;
    public float lastTimeCast = -100;

    #region State
    public DeathBringerIdleState idleState {  get; private set; }
    public DeathBringerBattleState battleState {  get; private set; }
    public DeathBringerAttackState attackState { get; private set; }
    public DeathBringerCastState castState { get; private set; }
    public DeathBringerTeleportState teleportState { get; private set; }
    public DeathBringerDeadState deadState { get; private set; }

    #endregion

    [Header("Teleport details")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 surroundingCheckSize;
    public float chanceToTeleport;
    public float defaultChanceToTeleport = 25;



    protected override void Awake()
    {
        base.Awake();
        idleState = new DeathBringerIdleState(this, stateMachine, "Idle", this);
        battleState = new DeathBringerBattleState(this, stateMachine, "Move", this);
        attackState = new DeathBringerAttackState(this, stateMachine, "Attack", this);
        castState = new DeathBringerCastState(this, stateMachine, "Cast", this);
        teleportState = new DeathBringerTeleportState(this, stateMachine, "Teleport", this);
        deadState = new DeathBringerDeadState(this, stateMachine, "Idle", this);

        SetupDefaultFacingDir(-1);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        chanceToTeleport = defaultChanceToTeleport;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.changeState(deadState);
    }

    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (cd.size.y / 2));

        if (!GroundBelow() || SomethingIsAround())
        {
            Debug.Log("No Position Found");
            FindPosition();
        }
    }

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    private bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }

    public bool CanTeleport()
    {
        if (Random.Range(0, 100) <= chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }

        return false;
    }

    public bool CanCast()
    {
        if (Time.time >= lastTimeCast + castStateCooldown)
        {
            return true;
        }

        return false;
    }

    public void Cast()
    {
        Player player = PlayerManager.instance.player;

        float xOffset = 0;
        if (player.rb.velocity.x != 0)
            xOffset = player.facingDir * castOffset.x;
        Vector3 castPosition = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + castOffset.y);
        GameObject newcast = Instantiate(castPrefab, castPosition, Quaternion.identity);
        newcast.GetComponent<DeathBringerCastController>().SetUpCast(stats);
    }
}
