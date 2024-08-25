using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        playerStats.IncreaseStatBy(buffAmount, buffDuration, playerStats.GetStat(buffType));
    }

    
}
