using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]

public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    public ItemEffect[] itemEffects;

    [Header("Item Cooldown")]
    public float itemCooldown;

    [Header("Major Stats")]
    public float strengeth; 
    public float agility; 
    public float Intelligence; 
    public float vitality; 

    [Header("Offensive Stats")]
    public float phsicalDamage;
    public float critChance;
    public float critPower; 

    [Header("Defensive Stats")]
    public float maxHealth;
    public float armor;
    public float evasion;
    public float magicResistance;

    [Header("Magic Stats")]
    public float fireDamge;
    public float iceDamage;
    public float shockDamage;

    [Header("Craft Requirements")]
    public List<InventoryItem> craftingMaterials;

    public void Effect(Transform _executeTransform)
    {
        foreach (var effect in itemEffects)
        {
            effect.ExecuteEffect(_executeTransform);
        }
    }

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strengeth.AddModifier(strengeth);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(Intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.phsicalDamage.AddModifier(phsicalDamage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamge.AddModifier(fireDamge);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.shockDamage.AddModifier(shockDamage);
    }

    public void RemoveModifiers() {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strengeth.RemoveModifier(strengeth);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(Intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.phsicalDamage.RemoveModifier(phsicalDamage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamge.RemoveModifier(fireDamge);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.shockDamage.RemoveModifier(shockDamage);
    }
}
