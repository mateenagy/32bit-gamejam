using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnersGround : GunnersState
{
    public GunnersGround(GunnersSM stateMachine, GunnersFactory factory) : base(stateMachine, factory)
    {
        IsRoot = true;
    }

    public override void Enter()
    {
        base.Enter();
        InitialSubState();
    }

    public override void Update()
    {
        base.Update();
        CheckSwitchState();
    }

    public override void InitialSubState()
    {
        base.InitialSubState();
        SetSubState(Factory.States[GunnersStates.Patrol]);
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
