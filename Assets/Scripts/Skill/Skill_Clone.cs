using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Clone : Skill
{

    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;

    // These values will be locked or unlocked by skill tree
    [SerializeField] private bool createCloneOnStart; 
    [SerializeField] private bool createCloneOnOver;
    [SerializeField] private bool createCloneOnParry;
    [Header("Clone Duplication")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float duplicateChance;

    [Header("Crystal As Clone")]
    [SerializeField] private bool crystalAsClone;


    public void CreateClone(Transform _cloneTransform, Vector3 _offset = default)
    {
        if (crystalAsClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Skill_Clone_Controller>().SetupClone(_cloneTransform, cloneDuration, canAttack, _offset, FindClosestEnemy(_cloneTransform), canDuplicateClone, duplicateChance);
    }

    public void CreateCloneOnDashStart()
    {
        if (createCloneOnStart)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashOver()
    {
        if (createCloneOnOver)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnParry(Transform _enemyTransform, float _delay)
    {
        if (createCloneOnParry)
        {
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDir, 0), _delay));
        }
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        CreateClone(_transform.transform, _offset);
    }

    public bool GetCrystalAsClone() => crystalAsClone;
}
