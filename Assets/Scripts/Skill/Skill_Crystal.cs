using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Crystal : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    private GameObject currentCrystal;

    [Header("Crystal")]
    [SerializeField] public UI_SkillTreeSlot crystalUnlockButton;
    public bool crystalUnlocked {  get; private set; }

    [Header("Crystal Blink")]
    [SerializeField] private UI_SkillTreeSlot crystalBlinkUnlockButton;
    public bool crystalBlinkUnlocked { get; private set; }

    [Header("Crystal Explosion")]
    [SerializeField] private UI_SkillTreeSlot crystalExplosionUnlockButton;
    public bool crystalExplosionUnlocked { get; private set; }

    [Header("Controlled Destruction")]
    [SerializeField] private UI_SkillTreeSlot crystalControlledDestructionUnlockButton;
    [SerializeField] private float crystalMoveSpeed;
    public bool crystalControlledDestructionUnlocked { get; private set; }

    [Header("Multiple Crystal")]
    [SerializeField] private UI_SkillTreeSlot multipleCrystalUnlockButton;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();
    public bool multipleCrystalUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();

        crystalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        crystalBlinkUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalBlink);
        crystalExplosionUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalExplosion);
        crystalControlledDestructionUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalControlledDestruction);
        multipleCrystalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultipleCrystal);

    }

    #region unlock skill region
    private void UnlockCrystal()
    {
        crystalUnlocked = crystalUnlockButton.unlocked;
    }

    private void UnlockCrystalBlink()
    {
        crystalBlinkUnlocked = crystalBlinkUnlockButton.unlocked;
    }

    private void UnlockCrystalExplosion()
    {
        crystalExplosionUnlocked = crystalExplosionUnlockButton.unlocked;
    }

    private void UnlockCrystalControlledDestruction()
    {
        crystalControlledDestructionUnlocked = crystalControlledDestructionUnlockButton.unlocked;
    }

    private void UnlockMultipleCrystal()
    {
        multipleCrystalUnlocked = multipleCrystalUnlockButton.unlocked;
    }
    #endregion

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
            return;

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (crystalControlledDestructionUnlockButton.unlocked) // If we dont want player can swap position when crystal is moving, some kind of getting but losing,
                return;                // then just constrain this ability.

            Vector2 playerPosition = player.transform.position;

            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPosition;

            if (crystalBlinkUnlockButton.unlocked)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            currentCrystal.GetComponent<Skill_Crystal_Controller>()?.FinishCrystal();
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Skill_Crystal_Controller currentCrystalScript = currentCrystal.GetComponent<Skill_Crystal_Controller>();

        currentCrystalScript.SetupCrystal(crystalDuration, crystalExplosionUnlockButton.unlocked, crystalControlledDestructionUnlockButton.unlocked, crystalMoveSpeed, FindClosestEnemy(currentCrystal.transform));
    }

    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Skill_Crystal_Controller>().ChooseRandomEnemy();
    public void CurrentCrystalAssignTarget(Transform _target) => currentCrystal.GetComponent<Skill_Crystal_Controller>().AssignTargetEnemy(_target);

    private bool CanUseMultiCrystal()
    {
        if (multipleCrystalUnlockButton.unlocked)
        {
            if (crystalLeft.Count == amountOfStacks)
            {
                Invoke(nameof(ResetAbility), useTimeWindow);
            }

            cooldown = 0;
            // respawn crystal
            if (crystalLeft.Count > 0)
            {
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);
                newCrystal.GetComponent<Skill_Crystal_Controller>().
                    SetupCrystal(crystalDuration, crystalExplosionUnlockButton.unlocked, crystalControlledDestructionUnlockButton.unlocked, crystalMoveSpeed, FindClosestEnemy(newCrystal.transform));
                if (crystalLeft.Count <= 0)
                {
                    // cooldown the skill and refill the crystal
                    cooldown = multiStackCooldown;
                    RefillCrystal();
                }
                return true;
            }
        }
        cooldown = 0;
        return false;
    }

    private void RefillCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++) 
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = multiStackCooldown;
        RefillCrystal();
    }
}
