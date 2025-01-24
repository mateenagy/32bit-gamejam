using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwarmerStates
{
    Idle,
    JumpAttack,
}
public class SwarmerFactory : Factory<SwarmerStates, SwarmerState, SwarmerSM>
{
    Dictionary<SwarmerStates, SwarmerState> _States = new();
    public override Dictionary<SwarmerStates, SwarmerState> States => _States;

    public SwarmerFactory(SwarmerSM sm) : base(sm)
    {
        States.Add(SwarmerStates.Idle, new SwarmerIdle(sm, this));
        States.Add(SwarmerStates.JumpAttack, new SwarmerJumpAttack(sm, this));
    }
}
