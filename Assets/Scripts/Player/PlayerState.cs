using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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

    protected List<Transition> transitions = new List<Transition>();

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void SetupTransitions()
    {
        // 子类可以重写此方法来设置过渡条件
    }

    public virtual void Enter()
    {
        //Debug.Log("I am Enter in " +  this.animBoolName);
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }
    public virtual void Exit()
    {
        //Debug.Log("I am Exit in " + this.animBoolName);
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void Update()
    {
        //Debug.Log("I am Update in " + this.animBoolName);

        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.velocity.y);

        // 检查所有过渡条件
        foreach (var transition in transitions)
        {
            try
            {
                if (transition.condition())
                {
                    // 如果有额外操作，执行它
                    transition.onTransition?.Invoke();
                    Debug.Log($"尝试从 {animBoolName} 状态切换到 {transition.targetState.animBoolName} 状态");
                    stateMachine.changeState(transition.targetState);
                    break;
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"[Error]在从 {animBoolName} 状态切换到 {transition.targetState.animBoolName} 状态时出错: {ex.Message}");
            }
        }
    }


    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
