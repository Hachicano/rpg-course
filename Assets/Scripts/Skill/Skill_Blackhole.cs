using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Blackhole : Skill
{
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float blackholeDuration;
    [SerializeField] public float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [Space]
    [SerializeField] private int AttackTimes;
    [SerializeField] private float cloneAttackCooldown;

    public Skill_Blackhole_Controller currentBlackhole;
    public bool haveBlackhole;


    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);

        currentBlackhole = newBlackhole.GetComponent<Skill_Blackhole_Controller>();

        currentBlackhole.SetupBlackhole(blackholeDuration, maxSize, growSpeed, shrinkSpeed, AttackTimes, cloneAttackCooldown);

        haveBlackhole = true;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool BlackholeSkillFinish()
    {
        if (!currentBlackhole) 
            return false;

        if (currentBlackhole.playerCanExitTheState)
        {
            currentBlackhole = null;
            return true;
        }
        return false;
    }

    public float GetBlackholeRadius() => maxSize / 2;
}
