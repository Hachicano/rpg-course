using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Dodge : Skill
{
    [Header("Dodge")]
    [SerializeField] private int evasionAmountFromDodge;
    private bool added;
    public UI_SkillTreeSlot dodgeUnlockButton;
    public bool dodgeUnlocked { get; private set; }

    [Header("Dodge Mirage")]
    [SerializeField] private UI_SkillTreeSlot dodgeMirageUnlockButton;
    public bool dodgeMirageUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();

        dodgeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        dodgeMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDodgeMirage);
    }

    private void UnlockDodge()
    {
        if (dodgeUnlockButton.unlocked && !added)
        {
            player.stats.evasion.AddModifier(evasionAmountFromDodge);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
            added = true;
        }
        else if (!dodgeUnlockButton.unlocked && added)
        {
            player.stats.evasion.RemoveModifier(evasionAmountFromDodge);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = false;
            added = false;
        }
    }

    private void UnlockDodgeMirage()
    {
        if (dodgeMirageUnlockButton.unlocked)
        {
            dodgeMirageUnlocked = true;
        }
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlockButton.unlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDir, 0)); 
        }
    }

}
