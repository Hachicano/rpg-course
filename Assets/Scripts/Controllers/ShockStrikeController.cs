using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrikeController : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;
    private float damage;

    private Animator anim;
    private bool triggered;

    public void Setup(float _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetStats)
        {
            Destroy(gameObject);
            return;
        }


        if (triggered)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f && !triggered)
        {
            anim.transform.localPosition = new Vector3(0, .5f);
            anim.transform.localRotation = Quaternion.identity;

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);

            Invoke("DamageAndSelfDestroy", .2f);
            anim.SetTrigger("Hit");
            triggered = true;
        }
    }

    private void DamageAndSelfDestroy()
    {
        targetStats.TakeDamage(damage);
        targetStats.ApplyShock(true);
        Destroy(gameObject, .4f);
    }
}
