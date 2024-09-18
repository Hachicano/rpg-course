using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    public string id;
    public bool actiationStatus;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Generate checkpoint id")] 
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            ActivateCheckPoint();
        }
    }

    public void ActivateCheckPoint()
    {
        if (!actiationStatus)
            AudioManager.instance.PlayerSFX(5, transform);  // sfx_checkpoint
        actiationStatus = true;
        anim.SetBool("active", true);
    }
}
