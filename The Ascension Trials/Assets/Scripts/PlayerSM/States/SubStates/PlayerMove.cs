using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerState
{
    public PlayerMove(PlayerSM stateMachine, PlayerFactory factory) : base(stateMachine, factory)
    {
        IsRoot = false;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        Vector3 finalMove = Ctx.PlayerTransform.right * Ctx.MoveDirection.x + Ctx.PlayerTransform.forward * Ctx.MoveDirection.y;
        Ctx.Controller.Move(Ctx.Speed * Time.deltaTime * finalMove);
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
        if (!Ctx.IsMoving)
        {
            SwitchState(Factory.States[PlayerStates.Idle]);
        }
    }
}
