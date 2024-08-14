using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Crystal : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    private GameObject currentCrystal;

    [Header("Crystal Mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Explosive Crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float crystalMoveSpeed;

    [Header("Multi Stacking Crystal")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();


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
            if (canMoveToEnemy) // If we dont want player can swap position when crystal is moving, some kind of getting but losing,
                return;                // then just constrain this ability.

            Vector2 playerPosition = player.transform.position;

            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPosition;

            if (cloneInsteadOfCrystal)
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

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, crystalMoveSpeed, FindClosestEnemy(currentCrystal.transform));
    }

    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Skill_Crystal_Controller>().ChooseRandomEnemy();
    public void CurrentCrystalAssignTarget(Transform _target) => currentCrystal.GetComponent<Skill_Crystal_Controller>().AssignTargetEnemy(_target);

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
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
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, crystalMoveSpeed, FindClosestEnemy(newCrystal.transform));
                if (crystalLeft.Count <= 0)
                {
                    // cooldown the skill and refill the crystal
                    cooldown = multiStackCooldown;
                    RefillCrystal();
                }
                return true;
            }
        }
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
