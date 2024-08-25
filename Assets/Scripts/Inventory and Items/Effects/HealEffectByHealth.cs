using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item_Effect/Heal_Effect")]
public class HealEffectByHealth : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercentage;

    public override void ExecuteEffect(Transform _executeTransform)
    {
        // get playerstat
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        // decide how much to heal
        float healAmount = Mathf.Round(playerStats.GetTotalMaxHealthValue() * healPercentage);

        // actually heal
        ///////////////////////////////////////////////
        /// playerStats.currentHealth += healAmount ///  It's not exactly right to do !!!!!!!!!!!!!!!!!
        ///////////////////////////////////////////////
        playerStats.IncreaseHealthBy(healAmount);
    }
}
