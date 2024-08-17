using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Clone_Controller : MonoBehaviour
{
    

    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLosingSpeed;

    private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;
    private Transform closestEnemy;
    private int facingDir = 1;

    private bool canDuplicateClone;
    private float duplicateChance;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0 )
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed));
            if (sr.color.a <= 0 )
                Destroy(gameObject);

        }
    }

    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicateClone, float _duplicateChance)
    {
        if (_canAttack )
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 4)); //Random.range������Сֵ�����������ֵ
        }

        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;
        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicateClone;
        duplicateChance = _duplicateChance;

        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                PlayerManager.instance.player.stats.DoDamage(hit.GetComponent<CharacterStats>());
                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < duplicateChance)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(.6f * facingDir, 0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget() //maybe ���Ǻ�ʡ���ܵ�д���� ���Ըĳ�ǰ���������߼�⣬�ֱ�Ƚ��䵽�ĵ�һ�����˵ľ���
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
