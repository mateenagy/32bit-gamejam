using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunnersStates
{
    Ground,
    Patrol,
    Attack
}
public class GunnersFactory : Factory<GunnersStates, GunnersState, GunnersSM>
{
    Dictionary<GunnersStates, GunnersState> _States = new();
    public override Dictionary<GunnersStates, GunnersState> States => _States;

    public GunnersFactory(GunnersSM sm) : base(sm)
    {
        States.Add(GunnersStates.Ground, new GunnersGround(sm, this));
        States.Add(GunnersStates.Patrol, new GunnersPatrol(sm, this));
        States.Add(GunnersStates.Attack, new GunnersAttack(sm, this));
    }
}
