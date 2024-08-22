using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice and Fire Effect", menuName = "Data/Item_Effect/Ice_and_Fire")]

public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private Vector2 newVelocity;

    public override void ExecuteEffect(Transform _executeTransform)
    {
        Player player = PlayerManager.instance.player;

        bool isThirdAttack = player.primaryAttack.comboCounter == 2;

        if (isThirdAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _executeTransform.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(newVelocity.x * player.facingDir, newVelocity.y);
            Destroy(newIceAndFire, 2f);
        }

        /*
        Skill_Blackhole blackhole = SkillManager.instance.blackhole;

        if (blackhole.haveBlackhole)
        {
            if (blackhole.currentBlackhole.cloneAttackRelease)
            {
                GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _executeTransform.position, player.transform.rotation);
                newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(newVelocity.x * player.facingDir, newVelocity.y);
                Destroy(newIceAndFire, 2f);
            }
        }
        */
    }
}
