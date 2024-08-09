using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    #endregion

    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration = 0.07f;
    protected bool isKnocked;

    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;


    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    protected virtual void Awake()
    {
        fx = GetComponent<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }

    public virtual void Damage()
    {
        fx.StartCoroutine(nameof(fx.FlashFX));
        StartCoroutine(nameof(HitKnockback));
        Debug.Log(gameObject.name + " was damaged!");
    }

    protected virtual IEnumerator HitKnockback() { 
        isKnocked = true;

        rb.velocity = new Vector2(knockbackDirection.x * PlayerManager.instance.player.primaryAttack.attackDir, knockbackDirection.y); // 不知道为啥效果正常但是逻辑是反的

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }

    #region Collision

    public virtual bool IsGoundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);


    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion
    #region Velocity
    public virtual void setZeroVelocity() => setVelocity(0, 0);

    public virtual void setVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
        {
            return;
        }

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion
    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion
}
