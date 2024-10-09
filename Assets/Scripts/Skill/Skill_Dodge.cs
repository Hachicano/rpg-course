using UnityEngine;
using UnityEngine.UI;

public class Skill_Dodge : Skill
{
    [Header("Dodge")]
    [SerializeField] private int evasionAmountFromDodge;
    [SerializeField] private bool added;
    public bool reloadAdd;
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

    protected override void Update()
    {
        base.Update();
        if (reloadAdd)
        {
            player.stats.evasion.AddModifier(evasionAmountFromDodge);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
            added = true;
            reloadAdd = false;
            Debug.Log("reload doge");
        }
    }

    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockDodgeMirage();
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
        dodgeMirageUnlocked = dodgeMirageUnlockButton.unlocked;
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDir, 0));
        }
    }

}
