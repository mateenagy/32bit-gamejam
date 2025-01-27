using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunnersState : State<GunnersSM, GunnersFactory>
{
    public GunnersState(GunnersSM stateMachine, GunnersFactory factory) : base(stateMachine, factory)
    {
        Ctx = stateMachine;
        Factory = factory;
    }

    protected override void SwitchState(State<GunnersSM, GunnersFactory> newState)
    {
        base.SwitchState(newState);
        if (IsRoot)
        {
            Ctx.CurrentState = (GunnersState)newState;
        }
        else
        {
            CurrentSuperState.SetSubState(newState);
        }
    }
}
