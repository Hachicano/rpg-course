using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    [Header("Major Stats")]
    private float defaultStrengeth;
    private float defaultAgility;
    private float defaultIntelligence;
    private float defaultVitality;

    [Header("Offensive Stats")]
    private float defaultPhsicalDamage;
    private float defaultCritChance;
    private float defaultCritPower; 

    [Header("Defensive Stats")]
    private float defaultMaxHealth;
    private float defaultArmor;
    private float defaultEvasion;
    private float defaultMagicResistance;

    [Header("Magic Stats")]
    private float defaultFireDamage;
    private float defaultIceDamage;
    private float defaultShockDamage;

    private Enemy enemy;
    private ItemDrop myDropSystem;
    public Stat soulsDropAmount;
    public float souls = 100;

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .1f;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
        soulsDropAmount.SetBaseValue(souls);

        getDefaultStats();
    }

    protected override void Start()
    {
        ApplyLevelModifiers();
        base.Start();
    }

    private void getDefaultStats()
    {
        defaultStrengeth = strengeth.GetValue();
        defaultAgility = agility.GetValue();
        defaultIntelligence = intelligence.GetValue();
        defaultVitality = vitality.GetValue();
        defaultPhsicalDamage = phsicalDamage.GetValue();
        defaultCritChance = critChance.GetValue();
        defaultCritPower = critPower.GetValue();
        defaultMaxHealth = maxHealth.GetValue();
        defaultArmor = armor.GetValue();
        defaultEvasion = evasion.GetValue();
        defaultMagicResistance = magicResistance.GetValue();
        defaultFireDamage = fireDamage.GetValue();
        defaultIceDamage = iceDamage.GetValue();
        defaultShockDamage = shockDamage.GetValue();
    }

    private void ApplyLevelModifiers()
    {
        Modify(strengeth);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);
        Modify(phsicalDamage);
        Modify(critChance);
        Modify(critPower);
        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);
        Modify(fireDamage);
        Modify(iceDamage);
        Modify(shockDamage);
        Modify(soulsDropAmount);
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;
            modifier = Mathf.Round(modifier);
            _stat.AddModifier(modifier);
        }
    }

    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
        myDropSystem.GenerateDrop();
        int dropCurrency = Mathf.RoundToInt(soulsDropAmount.GetValue());
        PlayerManager.instance.currency += dropCurrency;

        // 启动回池协程
        StartCoroutine(ReturnToPoolAfterDelay());
        //Destroy(gameObject, 5f);
    }
    IEnumerator ReturnToPoolAfterDelay()
    {
        // 等待 5 秒
        yield return new WaitForSeconds(5f);

        // 将当前游戏对象返回对象池
        ObjectPoolManager.instance.returnToPool(gameObject);
    }

    public void resetStats()
    {
        Reset(strengeth, defaultStrengeth);
        Reset(agility, defaultAgility);
        Reset(intelligence, defaultIntelligence);
        Reset(vitality, defaultVitality);
        Reset(phsicalDamage, defaultPhsicalDamage);
        Reset(critChance, defaultCritChance);
        Reset(critPower, defaultCritPower);
        Reset(maxHealth, defaultMaxHealth);
        Reset(armor, defaultArmor);
        Reset(evasion, defaultEvasion);
        Reset(magicResistance, defaultMagicResistance);
        Reset(fireDamage, defaultFireDamage);
        Reset(iceDamage, defaultIceDamage);
        Reset(shockDamage, defaultShockDamage);

        ApplyLevelModifiers();

        isIgnited = false;
        isChilled = false;
        isShocked = false;
        isInvincible = false;
        isVulnerable = false;
        isDead = false;

        currentHealth = GetTotalMaxHealthValue();
    }

    private void Reset(Stat _stat, float _default)
    {
        _stat.SetBaseValue(default);
        _stat.modifiers.Clear();
    }
}
