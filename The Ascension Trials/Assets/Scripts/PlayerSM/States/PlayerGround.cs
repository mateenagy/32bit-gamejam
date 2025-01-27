using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGround : PlayerState
{
    public PlayerGround(PlayerSM stateMachine, PlayerFactory factory) : base(stateMachine, factory)
    {
        IsRoot = true;
    }

    public override void Enter()
    {
        base.Enter();
        InitialSubState();
    }

    public override void InitialSubState()
    {
        base.InitialSubState();
        if (!Ctx.IsMoving)
        {
            SetSubState(Factory.States[PlayerStates.Idle]);
        }
        else
        {
            SetSubState(Factory.States[PlayerStates.Move]);
        }
    }

    public override void Update()
    {
        base.Update();
        CheckSwitchState();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void CheckSwitchState()
    {
        base.CheckSwitchState();
    }
}
