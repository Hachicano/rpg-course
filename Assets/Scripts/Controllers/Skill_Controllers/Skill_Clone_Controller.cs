using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Clone_Controller : MonoBehaviour
{
    

    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLosingSpeed;

    private float cloneTimer;
    private float attackMultiplier;
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

    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicateClone, float _duplicateChance, float _attackMultiplier)
    {
        if (_canAttack )
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 4)); //Random.range包括最小值，不包括最大值
        }

        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;
        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicateClone;
        duplicateChance = _duplicateChance;
        attackMultiplier = _attackMultiplier;

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
                Player player = PlayerManager.instance.player;
                // PlayerManager.instance.player.stats.DoDamage(hit.GetComponent<CharacterStats>());
                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDoDamage(enemyStats, attackMultiplier);

                if (player.skill.clone.canApplyOnHitEffect)
                {
                    // inventory get weapon call item effect
                    ItemData_Equipment equipedWeapon = Inventory.instance.GetEquipment(EquipmentType.Weapon);
                    ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
                    if (equipedAmulet != null)
                    {
                        equipedAmulet.Effect(hit.transform);
                    }
                    else if (equipedWeapon != null)
                    {
                        equipedWeapon.Effect(hit.transform);
                    }
                }

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

    private void FaceClosestTarget() //maybe 不是很省性能的写法， 可以改成前后两条射线检测，分别比较射到的第一个敌人的距离
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
