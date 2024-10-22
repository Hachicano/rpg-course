using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringer_AnimationTrigger : Enemy_AnimationTrigger
{
    private Enemy_DeathBringer enemy => GetComponentInParent<Enemy_DeathBringer>();

    private void Relocate() => enemy.FindPosition();

    private void MakeDBInvisible() => enemy.fx.MakeTransparent(true);
    private void MakeDBVisible() => enemy.fx.MakeTransparent(false);
}
