using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        player.Die();
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    public override void DecreaseHealthBy(float _damage)
    {
        base.DecreaseHealthBy(_damage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (currentArmor != null)
        {
            currentArmor.Effect(player.transform);
        }
    }
}