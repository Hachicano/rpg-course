using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Blackhole : Skill
{
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float blackholeDuration;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [Space]
    [SerializeField] private int AttackTimes;
    [SerializeField] private float cloneAttackCooldown;

    public Skill_Blackhole_Controller currentBlackhole;


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
}