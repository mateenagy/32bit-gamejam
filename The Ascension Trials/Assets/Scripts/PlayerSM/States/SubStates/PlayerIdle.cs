using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    public PlayerIdle(PlayerSM stateMachine, PlayerFactory factory) : base(stateMachine, factory)
    {
        IsRoot = false;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log($"STATE: Idle");
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
        if (Ctx.IsMoving)
        {
            SwitchState(Factory.States[PlayerStates.Move]);
        }
    }
}
