using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    private string animParaName;

    protected float xInput;

    protected Rigidbody2D rb;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animParaName = _animBoolName;
    }

    public virtual void Enter()
    {
        //Debug.Log("I am Enter in " +  this.animParaName);
        player.anim.SetBool(animParaName, true);
        rb = player.rb;
    }

    public virtual void Update()
    {
        //Debug.Log("I am Update in " + this.animParaName);
        xInput = Input.GetAxisRaw("Horizontal");
        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animParaName, false);
        //Debug.Log("I am Exit in " + this.animParaName);
    }
}
