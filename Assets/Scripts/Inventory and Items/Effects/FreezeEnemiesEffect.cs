using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze enemies effect", menuName = "Data/Item_Effect/Freeze_enemies")]
public class FreezeEnemiesEffect : ItemEffect
{
    [SerializeField] private float freezeDuration;

    public override void ExecuteEffect(Transform _executeTransform)
    {
        if (!Inventory.instance.canUseArmor())
            return;

        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerStats.currentHealth > playerStats.GetTotalMaxHealthValue() * .1f)
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_executeTransform.position, 5);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(freezeDuration);
        }
    }
}
