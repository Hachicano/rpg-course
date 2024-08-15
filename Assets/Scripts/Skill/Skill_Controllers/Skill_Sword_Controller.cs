using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Sword_Controller : MonoBehaviour
{
    private float returnSpeed;
    private float swordDispearDistancce = 1f;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    private float freezeTimeDuration;

    [Header("Bounce Info")]
    private float bounceSpeed;
    private int bounceAmount;
    private bool canBounce;

    [Header("Pierce Info")]
    private int pierceAmount;

    [Header("Spin Info")]
    private bool isSpinning;
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool firstStop = true;
    private float spinDirection;

    private float hitTimer;
    private float hitCooldown;


    private List<Transform> enemyTarget; // If you have it public, it will be created on its own by Unity. But if it private, there
                                         // is no one to create it and give it a default value. So we should create a default empty value by ourselves.
    private int targetIndex;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < swordDispearDistancce)
            {
                player.CatchTheSword();
                isReturning = false;
            }
        }

        BounceLogic();
        SpinLogic();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            DamagAndFreeze(enemy);
        }

        SetupTargetForBounce(collision);

        StuckInto(collision);
    }
    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount >0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (canBounce && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {
        player = _player;
        rb.velocity = _dir; ;
        rb.gravityScale = _gravityScale;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;

        if (pierceAmount <= 0)
        {
            anim.SetBool("Rotation", true);
        }
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 7);
    }
    public void SerupBounce(bool _canBounce, int _amountOfBounce, float _bounceSpeed)
    {
        canBounce = _canBounce;
        bounceAmount = _amountOfBounce;
        enemyTarget = new List<Transform>();
        bounceSpeed = _bounceSpeed;
    }
    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }
    public void SetupSpine(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    public void ReturnSword()
    {
        //rb.isKinematic=false;  // If I set up isKinematic to false, the sword might can't retun when it goes too far away.
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
        canRotate = true;
        anim.SetBool("Rotation", true);
    }
    private void BounceLogic()
    {
        if (canBounce && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                enemyTarget[targetIndex].GetComponent<Enemy>().DamageEffect();
                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    canBounce = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }
    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, 
                    new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime); 

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            hit.GetComponent<Enemy>().DamageEffect();
                        }
                    }
                }
            }
        }
    }

    private void SetupTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (canBounce && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void DamagAndFreeze(Enemy enemy)
    {
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
        enemy.DamageEffect();
    }
    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        if (firstStop)
        {
            spinTimer = spinDuration;
            firstStop = false;
        }
    }
    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}
