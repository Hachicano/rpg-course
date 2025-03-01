using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition
{
    public PlayerState targetState { get; }
    public Func<bool> condition { get; }
    public Action onTransition { get; }

    public Transition(PlayerState _targetState, Func<bool> _condition, Action _onTransition = null)
    {
        targetState = _targetState;
        condition = _condition;
        onTransition = _onTransition;
    }
}