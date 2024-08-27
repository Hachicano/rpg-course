using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Clone : Skill
{

    [Header("Time Mirage")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private float attackMultiplier;
    [Space]

    [Header("Clone Attack")]
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    public bool cloneAttackUnlocked { get; private set; }

    [Header("Aggresive Mirage")]
    [SerializeField] private UI_SkillTreeSlot aggresiveMirageUnlockButton;
    [SerializeField] private float aggresiveMirageMultiplier;
    public bool canApplyOnHitEffect { get; private set; }


    // These values will be locked or unlocked by skill tree
    [Header("Multiple Mirage")]
    [SerializeField] private UI_SkillTreeSlot multipleMirageUnlockButton;
    [SerializeField] private float multipleMirageMultiplier;
    [SerializeField] private float duplicateChance;
    public bool multipleMirageUnlocked { get; private set; }

    [Header("Crystal Mirage")]
    [SerializeField] private UI_SkillTreeSlot crystalMirageUnlockButton;
    public bool crystalMirageUnlocked { get; private set; }


    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggresiveMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveMirage);
        multipleMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultipleMirage);
        crystalMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
    }

    #region Unlock region

    private void UnlockCloneAttack()
    {
        cloneAttackUnlocked = cloneAttackUnlockButton.unlocked;
        if (cloneAttackUnlocked)
            attackMultiplier = cloneAttackMultiplier;
        else
            attackMultiplier = 0;
    }

    private void UnlockAggresiveMirage()
    {
        if (aggresiveMirageUnlockButton.unlocked)
        {
            attackMultiplier = aggresiveMirageMultiplier;
        }
        else
        {
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    private void UnlockMultipleMirage()
    {
        multipleMirageUnlocked = multipleMirageUnlockButton.unlocked;
        if (multipleMirageUnlocked)
        {
            attackMultiplier = multipleMirageMultiplier;
            canApplyOnHitEffect = true;
        }
        else
        { 
            attackMultiplier = aggresiveMirageMultiplier;
            canApplyOnHitEffect = false;
        }
    }

    private void UnlockCrystalMirage()
    {
        crystalMirageUnlocked = crystalMirageUnlockButton.unlocked;
    }

    #endregion

    public void CreateClone(Transform _cloneTransform, Vector3 _offset = default)
    {
        if (crystalMirageUnlocked)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Skill_Clone_Controller>().SetupClone(_cloneTransform, cloneDuration, cloneAttackUnlocked, _offset, FindClosestEnemy(_cloneTransform), multipleMirageUnlocked, duplicateChance, attackMultiplier);
    }

    public void CreateCloneOnParry(Transform _enemyTransform, float _delay)
    {
        StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDir, 0), _delay));
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        CreateClone(_transform.transform, _offset);
    }

    public bool GetCrystalAsClone() => crystalMirageUnlocked;
}
