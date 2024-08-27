using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Parry : Skill
{
    [Header("Parry")]
    public UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("Parry Restore")]
    [SerializeField] private UI_SkillTreeSlot parryRestoreUnlockButton;
    public bool parryRestoreUnlocked { get; private set; }
    [Range(0f, 1f)]
    [SerializeField] private float restorePercentage;

    [Header("Parry Mirage")]
    [SerializeField] private UI_SkillTreeSlot parryMirageUnlockButton;
    public bool parryMirageUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();

        if (parryRestoreUnlockButton.unlocked)
        {
            float restore = player.stats.GetTotalMaxHealthValue() * restorePercentage;
            int restoreAmount = Mathf.RoundToInt(restore);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        parryRestoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryMirage);
    }

    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
        {
            parryUnlocked = true;
        }
    }

    private void UnlockParryRestore()
    {
        if (parryRestoreUnlockButton.unlocked)
        {
            parryRestoreUnlocked = true;
        }
    }

    private void UnlockParryMirage()
    {
        if (parryMirageUnlockButton.unlocked)
        {
            parryMirageUnlocked = true;
        }

    }

    public void MakeMirageOnParry(Transform _transform, float _delay)
    {
        if (parryMirageUnlockButton.unlocked)
        {
            SkillManager.instance.clone.CreateCloneOnParry(_transform, _delay);
        }
    }
}
