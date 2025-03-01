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
        // ���������д�˷��������ù�������
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

        // ������й�������
        foreach (var transition in transitions)
        {
            try
            {
                if (transition.condition())
                {
                    // ����ж��������ִ����
                    transition.onTransition?.Invoke();
                    Debug.Log($"���Դ� {animBoolName} ״̬�л��� {transition.targetState.animBoolName} ״̬");
                    stateMachine.changeState(transition.targetState);
                    break;
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"[Error]�ڴ� {animBoolName} ״̬�л��� {transition.targetState.animBoolName} ״̬ʱ����: {ex.Message}");
            }
        }
    }


    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
