using System.Collections;
using System.Collections.Generic;
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


[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item_Effect/Buff_Effect")]

public class BuffEffect : ItemEffect
{
    private PlayerStats playerStats;
    [SerializeField] private StatType buffType;
    [SerializeField] private float buffAmount;
    // [SerializeField] private float buffPercentage;
    // [SerializeField] private float FinalBuff;
    [SerializeField] private float buffDuration;

    public override void ExecuteEffect(Transform _executeTransform)
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        // Stat statToModify = StatToModify();
        // PlayerManager.instance.player.GetComponent<PlayerStats>().  
        // Actually we can use percentage to give buff, but we must find statToMidify correctly
        // Such as we can use index to find the stat.

        playerStats.IncreaseStatBy(buffAmount, buffDuration, StatToModify());
    }

    private Stat StatToModify()
    {
        if (buffType == StatType.strengeth)
            return playerStats.strengeth;
        else if (buffType == StatType.agility)
            return playerStats.agility;
        else if (buffType == StatType.intelligence)
            return playerStats.intelligence;
        else if (buffType == StatType.vitality)
            return playerStats.vitality;
        else if (buffType == StatType.phsicalDamage)
            return playerStats.phsicalDamage;
        else if (buffType == StatType.critChance)
            return playerStats.critChance;
        else if (buffType == StatType.critPower)
            return playerStats.critPower;
        else if (buffType == StatType.maxHealth)
            return playerStats.maxHealth;
        else if (buffType == StatType.armor)
            return playerStats.armor;
        else if (buffType == StatType.evasion)
            return playerStats.evasion;
        else if (buffType == StatType.magicResistance)
            return playerStats.magicResistance;
        else if (buffType == StatType.fireDamge)
            return playerStats.fireDamge;
        else if (buffType == StatType.iceDamage)
            return playerStats.iceDamage;
        else if (buffType == StatType.shockDamage)
            return playerStats.shockDamage;

        return null;
    }
}
