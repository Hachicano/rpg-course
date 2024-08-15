using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stat strengeth; // 1 point increase damage by 1 and crit.power by 1%
    public Stat agility; // 1 point increase evasion by 0.5% and crit.chance by 1%
    public Stat Intelligence; // 1 point increase magic damage by 1 and magic resistence by 10%
    public Stat vitality; // 1 point increase Health by 3 or 5

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
    public Stat lightningDamage;

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;


    [SerializeField] private float currentHealth;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
        critPower.SetDefaultValue(150);

    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        float totalDamage = phsicalDamage.GetValue() + strengeth.GetValue();

        if (canCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        // _targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(_targetStats);
    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        float _fireDamage = fireDamge.GetValue();
        float _iceDamage = iceDamage.GetValue();
        float _lightningDamage = lightningDamage.GetValue();
        float totalMagicalDamge = Intelligence.GetValue();

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            totalMagicalDamge = CheckTargetMagicalResistance(_targetStats, totalMagicalDamge);
            _targetStats.TakeDamage(totalMagicalDamge);
            return;
        }

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while(!canApplyIgnite && !canApplyChill && !canApplyShock) { 
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
            else if (_lightningDamage != 0)
            {
                canApplyShock = true;
                break;
            }
        }

        if (canApplyIgnite)
            totalMagicalDamge += _fireDamage * 1.3f;
        else if (canApplyChill)
            totalMagicalDamge += _iceDamage * .7f;
        totalMagicalDamge = CheckTargetMagicalResistance(_targetStats, totalMagicalDamge);
        if (canApplyShock)
            totalMagicalDamge += _lightningDamage;

        _targetStats.TakeDamage(totalMagicalDamge);

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);

    }

    private static float CheckTargetMagicalResistance(CharacterStats _targetStats, float totalMagicalDamge)
    {
        float _iceResistance = _targetStats.iceDamage.GetValue() * .1f;
        float magicDamgeRemain = (100 - (_targetStats.magicResistance.GetValue() + _iceResistance + (_targetStats.Intelligence.GetValue() * .05f))) / 100;
        if (magicDamgeRemain <= .05f)
            magicDamgeRemain = .05f;
        totalMagicalDamge = totalMagicalDamge * magicDamgeRemain;
        totalMagicalDamge = Mathf.Round(totalMagicalDamge);
        return totalMagicalDamge;
    }

    public void ApplyAilments(bool _ignit, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked)
            return;
        isIgnited = _ignit;
        isChilled = _chill;
        isShocked = _shock;
    }


    public virtual void TakeDamage(float _damage)
    {
        currentHealth -= _damage;

        Debug.Log(_damage);

        if (currentHealth < 0)
        {
            currentHealth = 0;
            Die();
        }
    }
    private float CheckTargetArmor(CharacterStats _targetStats, float totalDamage)
    {
        float _iceResistance = _targetStats.iceDamage.GetValue() * .05f;
        totalDamage -= _targetStats.armor.GetValue() + _iceResistance;
        totalDamage = Mathf.Clamp(totalDamage, 0f, float.MaxValue);
        return totalDamage;
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        float totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue() * .5f;
        if (Random.Range(0, 100) < totalEvasion)
        {
            // Debug.Log("Attack avoided");
            return true;
        }
        return false;
    }
    private bool canCrit()
    {
        float totalCriticalChance = critChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    private float CalculateCritDamage(float _damage)
    {
        float totalCritPower = (critPower.GetValue() + strengeth.GetValue()) * .1f;

        float critDamage = _damage * totalCritPower;

        return Mathf.Round(critDamage);
    }

    protected virtual void Die()
    {

    }
}
