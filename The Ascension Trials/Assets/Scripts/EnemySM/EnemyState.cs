using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : State<EnemySM, EnemyFactory>
{
    public EnemyState(EnemySM stateMachine, EnemyFactory factory) : base(stateMachine, factory)
    {
        Ctx = stateMachine;
        Factory = factory;
    }

    protected override void SwitchState(State<EnemySM, EnemyFactory> newState)
    {
        base.SwitchState(newState);
        if (IsRoot)
        {
            Ctx.CurrentState = (EnemyState)newState;
        }
        else
        {
            CurrentSuperState.SetSubState(newState);
        }
    }
}
