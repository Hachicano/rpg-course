using System.Collections;
using UnityEngine;
public enum StatType
{
    strengeth,
    agility,
    intelligence,
    vitality,
    phsicalDamage,
    critChance,
    critPower,
    maxHealth,
    armor,
    evasion,
    magicResistance,
    fireDamge,
    iceDamage,
    shockDamage
}

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major Stats")]
    public Stat strengeth; // 1 point increase damage by 1 and crit.power by 10%
    public Stat agility; // 1 point increase evasion by 10% and crit chance by 10%
    public Stat intelligence; // 1 point increase magic damage by 1 and magic resistence by 10%
    public Stat vitality; // 1 point increase Health by 5

    [Header("Offensive Stats")]
    public Stat phsicalDamage;
    public Stat critChance;
    public Stat critPower; // default value 150%

    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat fireDamge;
    public Stat iceDamage;
    public Stat shockDamage;

    public bool isIgnited; // Dots damage over time
    public bool isChilled; // Increase critical chance by 30%, and slow everyspeed by 20%
    public bool isShocked; // Reduce accuracy by 20%

    private float IgnitedTimer;
    private float IgnitedDuration = 3f;
    private float IgniteDamageCooldown = .5f;
    private float IgniteDamageTimer;
    private float IgniteDamage;

    private float ChilledTimer;
    private float ChilledDuration = 3f;

    private float ShockedTimer;
    private float ShockedDuration = 6f;
    private float shockDamge;
    [SerializeField] private GameObject shockStrikePrefab;


    public float currentHealth;
    public System.Action onHealthChanged;
    private bool isVulnerable;
    public bool isDead { get; private set; }

    protected virtual void Awake()
    {
        fx = GetComponent<EntityFX>();
    }

    protected virtual void Start()
    {
        currentHealth = GetTotalMaxHealthValue();
        critPower.SetDefaultValue(150);

    }

    protected virtual void Update()
    {
        IgnitedTimer -= Time.deltaTime;
        ChilledTimer -= Time.deltaTime;
        ShockedTimer -= Time.deltaTime;
        IgniteDamageTimer -= Time.deltaTime;
        if (IgnitedTimer < 0)
        {
            isIgnited = false;
            IgniteDamage = 0;
        }
        if (ChilledTimer < 0)
        {
            isChilled = false;
        }
        if (ShockedTimer < 0)
        {
            isShocked = false;
        }
        ApplyIgniteDamage();
    }

    public void MakeVunerableFor(float _duration) => StartCoroutine(VulnerableCoroutine(_duration));

    private IEnumerator VulnerableCoroutine(float _duration)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(_duration);

        isVulnerable = false;
    }

    public virtual void IncreaseStatBy(float _modifier, float _duration, Stat _statToModify)
    {
        // start a coroutine for stat increase
        StartCoroutine(StatModifyCoroutine(_modifier, _duration, _statToModify));
    }

    private IEnumerator StatModifyCoroutine(float _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        float totalDamage = GetTotalPhisicalDamgeValue();

        if (canCrit(_targetStats))
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(float _damage)
    {
        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine(nameof(fx.FlashFX));

        if (currentHealth <= 0 && !isDead)
        {
            currentHealth = 0;
            Die();
        }
    }

    public virtual void DecreaseHealthBy(float _damage)
    {
        if (isVulnerable)
            _damage = _damage * 1.5f;
        _damage = Mathf.Round(_damage);
        Debug.Log(_damage);
        currentHealth -= Mathf.Round(_damage);

        if (onHealthChanged != null)
            onHealthChanged();
    }

    public virtual void IncreaseHealthBy(float _heal)
    {
        currentHealth += _heal;

        if (currentHealth > GetTotalMaxHealthValue())
            currentHealth = GetTotalMaxHealthValue();

        if (onHealthChanged != null)
            onHealthChanged();
    }

    #region Magical damage and Ailments
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        float _fireDamage = fireDamge.GetValue();
        float _iceDamage = iceDamage.GetValue();
        float _shockDamage = shockDamage.GetValue();
        float totalMagicalDamge = intelligence.GetValue();

        if (Mathf.Max(_fireDamage, _iceDamage, _shockDamage) <= 0)
        {
            totalMagicalDamge = CheckTargetMagicalResistance(_targetStats, totalMagicalDamge);
            _targetStats.TakeDamage(totalMagicalDamge);
            return;
        }

        AttemptToApplyAilment(_targetStats, _fireDamage, _iceDamage, _shockDamage, totalMagicalDamge);

    }

    private void AttemptToApplyAilment(CharacterStats _targetStats, float _fireDamage, float _iceDamage, float _shockDamage, float totalMagicalDamge)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _shockDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _shockDamage;
        bool canApplyShock = _shockDamage > _fireDamage && _shockDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .333334f && _fireDamage != 0) // Random.value gives you a number form 0 to 1
            {
                canApplyIgnite = true;
                break;
            }
            else if (Random.value < .5f && _iceDamage != 0)
            {
                canApplyChill = true;
                break;
            }
            else if (_shockDamage != 0)
            {
                canApplyShock = true;
                break;
            }
        }

        if (canApplyIgnite)
            totalMagicalDamge += _fireDamage * 1f;
        else if (canApplyChill)
            totalMagicalDamge += _iceDamage * .7f;
        totalMagicalDamge = CheckTargetMagicalResistance(_targetStats, totalMagicalDamge);
        if (canApplyShock)
            totalMagicalDamge += _shockDamage;

        _targetStats.TakeDamage(totalMagicalDamge);

        if (canApplyIgnite)
        {
            float fire = _fireDamage * .1f;
            fire = Mathf.Round(fire);
            _targetStats.SetupIgniteDamage(fire);
        }
        if (canApplyShock)
        {
            float shock = _shockDamage * .2f;
            shock = Mathf.Round(shock);
            _targetStats.SetupShockDamge(shock);
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAilments(bool _ignit, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignit && canApplyIgnite)
        {
            ApplyIgnite(_ignit);
        }

        if (_chill && canApplyChill)
        {
            ApplyChill(_chill);
        }

        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;
                // find closest enemy, and it should be only among the enemies
                // then instanciate ShunderStrike, and set up ThunderStrike
                HitClosestTargetWithShockStrike();
            }

        }
    }
    public void ApplyIgnite(bool _ignit)
    {
        isIgnited = _ignit;
        IgnitedTimer = IgnitedDuration;
        fx.IgniteFXFor(IgnitedDuration);
    }
    public void ApplyChill(bool _chill)
    {
        isChilled = _chill;
        ChilledTimer = ChilledDuration;

        float slowPercentage = .2f;
        GetComponent<Entity>().SlowEntity(slowPercentage, ChilledDuration);
        fx.ChillFxFor(ChilledDuration);
    }
    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;
        isShocked = _shock;
        ShockedTimer = ShockedDuration;
        fx.ShockFXFor(ShockedDuration);
    }

    public void SetupIgniteDamage(float _damage) => IgniteDamage = _damage;
    private void ApplyIgniteDamage()
    {
        if (IgniteDamageTimer < 0 && isIgnited)
        {
            float finaldamge = CheckTargetMagicalResistance(this, IgniteDamage);
            DecreaseHealthBy(finaldamge);
            if (currentHealth < 0 && !isDead)
            {
                Die();
                isIgnited = false;
            }
            IgniteDamageTimer = IgniteDamageCooldown;
        }
    }

    public void SetupShockDamge(float _damage) => shockDamge = _damage;
    private void HitClosestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > .1f)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = hit.transform;
                    closestDistance = distanceToEnemy;
                }
            }
        }

        if (closestEnemy == null)  // delete this if you dont want shocked target to be hit by shock strike
            closestEnemy = transform;

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrikeController>().Setup(shockDamge, closestEnemy.GetComponent<CharacterStats>());
        }
    }
    #endregion

    #region Stats Calculation
    protected float CheckTargetArmor(CharacterStats _targetStats, float totalDamage)
    {
        float _iceResistance = _targetStats.iceDamage.GetValue() * .05f;
        totalDamage -= _targetStats.armor.GetValue();
        if (IgniteDamage != 0 && _targetStats.iceDamage.GetValue() != 0)
            totalDamage -= _iceResistance;
        totalDamage = Mathf.Round(totalDamage);
        totalDamage = Mathf.Clamp(totalDamage, 0f, float.MaxValue);
        return totalDamage;
    }
    private float CheckTargetMagicalResistance(CharacterStats _targetStats, float totalMagicalDamge)
    {
        float iceResistance = _targetStats.iceDamage.GetValue() * .1f;
        float totalMagicalResistance = _targetStats.GetTotalMagicResistance();
        if (IgniteDamage != 0 && _targetStats.iceDamage.GetValue() != 0)
            totalMagicalResistance -= iceResistance;
        float magicDamgeRemain = (100 - totalMagicalResistance) / 100;
        if (magicDamgeRemain <= .05f)
            magicDamgeRemain = .05f;
        totalMagicalDamge = totalMagicalDamge * magicDamgeRemain;
        totalMagicalDamge = Mathf.Round(totalMagicalDamge);
        return totalMagicalDamge;
    }

    public virtual void OnEvasion()
    {

    }

    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        float totalEvasion = _targetStats.GetTotalEvasion();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            // Debug.Log("Attack avoided");
            _targetStats.OnEvasion();
            return true;
        }
        return false;
    }
    protected bool canCrit(CharacterStats _targetStats)
    {
        float totalCriticalChance = GetTotalCritChance();
        if (_targetStats.isChilled)
            totalCriticalChance += 30;
        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }
    protected float CalculateCritDamage(float _damage)
    {
        float totalCritPower = GetTotalCritPower() * .1f;

        float critDamage = _damage * totalCritPower;

        return Mathf.Round(critDamage);
    }

    //public float GetMaxHealthValue() => maxHealth.GetValue() + vitality.GetValue() * 5;
    #endregion

    #region Get Value
    public float GetTotalMaxHealthValue()
    {
        float finalMaxHealth = maxHealth.GetValue() + vitality.GetValue() * 5;
        // Debug.Log("Max Health: " + finalMaxHealth);
        return finalMaxHealth;
    }
    public float GetTotalPhisicalDamgeValue() => phsicalDamage.GetValue() + strengeth.GetValue();
    public float GetTotalCritChance() => critChance.GetValue() + agility.GetValue() * .1f;
    public float GetTotalCritPower() => critPower.GetValue() + strengeth.GetValue() * .1f;
    public float GetTotalMagicResistance()
    {
        float iceResistance = iceDamage.GetValue() * .1f;
        float totalMagicResistance = iceResistance + magicResistance.GetValue() + intelligence.GetValue() * .1f;
        return totalMagicResistance;
    }
    public float GetTotalEvasion() => evasion.GetValue() + agility.GetValue() * .1f;
    #endregion

    public Stat GetStat(StatType _statType)
    {
        if (_statType == StatType.strengeth)
            return strengeth;
        else if (_statType == StatType.agility)
            return agility;
        else if (_statType == StatType.intelligence)
            return intelligence;
        else if (_statType == StatType.vitality)
            return vitality;
        else if (_statType == StatType.phsicalDamage)
            return phsicalDamage;
        else if (_statType == StatType.critChance)
            return critChance;
        else if (_statType == StatType.critPower)
            return critPower;
        else if (_statType == StatType.maxHealth)
            return maxHealth;
        else if (_statType == StatType.armor)
            return armor;
        else if (_statType == StatType.evasion)
            return evasion;
        else if (_statType == StatType.magicResistance)
            return magicResistance;
        else if (_statType == StatType.fireDamge)
            return fireDamge;
        else if (_statType == StatType.iceDamage)
            return iceDamage;
        else if (_statType == StatType.shockDamage)
            return shockDamage;

        return null;
    }

    protected virtual void Die()
    {
        isDead = true;
    }

}
