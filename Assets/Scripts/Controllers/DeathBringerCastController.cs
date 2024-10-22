using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerCastController : MonoBehaviour
{
    [SerializeField] private Transform checkbox;
    [SerializeField] private Vector2 boxsize;
    [SerializeField] private LayerMask whatIsPlayer;

    private CharacterStats founderStat;

    public void SetUpCast(CharacterStats _founderStat)
    {
        founderStat = _founderStat;
    }

    private void AnimationCastTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(checkbox.position, boxsize, whatIsPlayer);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                founderStat.DoDamage(hit.GetComponent<CharacterStats>());
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(checkbox.position, boxsize);
    }

    private void SelfDestroy() => Destroy(gameObject);
}
