using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmerIdle : SwarmerState
{
    public SwarmerIdle(SwarmerSM stateMachine, SwarmerFactory factory) : base(stateMachine, factory)
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
        Ctx.Agent.SetDestination(Ctx.Player.transform.position);
        if (Vector3.Distance(Ctx.SwarmerTransform.position, Ctx.Player.transform.position) < Ctx.Agent.stoppingDistance)
        {
            Ctx.SwarmerAnimator.SetBool("isMoving", false);
            Ctx.Agent.isStopped = true;
            Ctx.Agent.velocity = Vector3.zero;
            Ctx.SwarmerTransform.LookAt(new Vector3(Ctx.Player.transform.position.x, Ctx.SwarmerTransform.position.y, Ctx.Player.transform.position.z));
            SwitchState(Factory.States[SwarmerStates.JumpAttack]);
        }
        else
        {
            Ctx.SwarmerAnimator.SetBool("isMoving", true);
        }
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
