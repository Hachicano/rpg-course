using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Sword_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 40f;
    [SerializeField] private float swordDispearDistancce = 1f;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

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
                player.ClearTheSword();
                isReturning = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetBool("Rotation", false);

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player)
    {
        player = _player;
        rb.velocity = _dir; ;
        rb.gravityScale = _gravityScale;

        anim.SetBool("Rotation", true);
    }

    public void ReturnSword()
    {
        rb.isKinematic=false;
        transform.parent = null;
        isReturning = true;
    }
}
