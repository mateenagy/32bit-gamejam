using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwarmerState : State<SwarmerSM, SwarmerFactory>
{
    public SwarmerState(SwarmerSM stateMachine, SwarmerFactory factory) : base(stateMachine, factory)
    {
        Ctx = stateMachine;
        Factory = factory;
    }

    protected override void SwitchState(State<SwarmerSM, SwarmerFactory> newState)
    {
        base.SwitchState(newState);
        if (IsRoot)
        {
            Ctx.CurrentState = (SwarmerState)newState;
        }
        else
        {
            CurrentSuperState.SetSubState(newState);
        }
    }
}
