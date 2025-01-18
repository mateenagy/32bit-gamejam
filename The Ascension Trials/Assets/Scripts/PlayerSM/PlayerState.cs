using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State<PlayerSM, PlayerFactory>
{
    public PlayerState(PlayerSM stateMachine, PlayerFactory factory) : base(stateMachine, factory)
    {
        Ctx = stateMachine;
        Factory = factory;
    }

    protected override void SwitchState(State<PlayerSM, PlayerFactory> newState)
    {
        base.SwitchState(newState);
        if (IsRoot)
        {
            Ctx.CurrentState = (PlayerState)newState;
        }
        else
        {
            CurrentSuperState.SetSubState(newState);
        }
    }
}
