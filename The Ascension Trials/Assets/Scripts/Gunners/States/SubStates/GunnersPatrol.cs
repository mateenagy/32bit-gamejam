using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GunnersPatrol : GunnersState
{
    Vector3 patrolPoint;
    public GunnersPatrol(GunnersSM stateMachine, GunnersFactory factory) : base(stateMachine, factory)
    {
        IsRoot = false;
    }

    public override void Enter()
    {
        base.Enter();
        Ctx.Agent.isStopped = false;
        Ctx.Agent.ResetPath();
        RandomPositionOnARadius(Ctx.Radius);
    }

    public override void Update()
    {
        base.Update();
        if (Ctx.CheckPlayerInSight())
        {
            if (IsDestinationReached())
            {
                SwitchState(Factory.States[GunnersStates.Attack]);
            }
        }
        else
        {
            if (IsDestinationReached())
            {
                RandomPositionOnARadius(Ctx.Radius);
            }
        }
        CheckSwitchState();
    }

    public override void Exit()
    {
        base.Exit();
        Ctx.Agent.isStopped = true;
    }

    public override void CheckSwitchState()
    {
        base.CheckSwitchState();
    }

    bool IsDestinationReached()
    {
        return Ctx.Agent.remainingDistance != Mathf.Infinity && Ctx.Agent.remainingDistance == 0;
    }

    void RandomPositionOnARadius(float radius)
    {
        Vector3 randomDirection = Random.onUnitSphere * radius;
        randomDirection += Ctx.Player.transform.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, 1);
        patrolPoint = hit.position;
        Ctx.Agent.SetDestination(patrolPoint);
    }
}
