using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyState
{
    public EnemyAttack(EnemySM stateMachine, EnemyFactory factory) : base(stateMachine, factory)
    {
        IsRoot = true;
    }

    public override void Enter()
    {
        base.Enter();
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
