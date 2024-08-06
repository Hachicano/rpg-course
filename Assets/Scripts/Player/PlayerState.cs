using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    private string animBoolName;

    protected float xInput;
    protected float yInput;

    protected float stateTimer;
    protected bool triggerCalled;

    protected Rigidbody2D rb;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        //Debug.Log("I am Enter in " +  this.animParaName);
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        //Debug.Log("I am Update in " + this.animParaName);

        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
        //Debug.Log("I am Exit in " + this.animParaName);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
