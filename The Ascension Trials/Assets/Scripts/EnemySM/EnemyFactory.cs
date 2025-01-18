using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    Basic,
}
public class EnemyFactory : Factory<EnemyStates, EnemyState, EnemySM>
{
    Dictionary<EnemyStates, EnemyState> _States = new();
    public override Dictionary<EnemyStates, EnemyState> States => _States;

    public EnemyFactory(EnemySM sm) : base(sm)
    {
        States.Add(EnemyStates.Basic, new EnemyPatrol(sm, this));
    }
}
