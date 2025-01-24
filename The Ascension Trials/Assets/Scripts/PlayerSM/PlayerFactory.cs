using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates
{
    /* ROOT STATES */
    Ground,
    Jump,
    /* SUB STATES */
    Fall,
    Idle,
    Move,
}
public class PlayerFactory : Factory<PlayerStates, PlayerState, PlayerSM>
{
    Dictionary<PlayerStates, PlayerState> _States = new();
    public override Dictionary<PlayerStates, PlayerState> States => _States;

    public PlayerFactory(PlayerSM sm) : base(sm)
    {
        States.Add(PlayerStates.Ground, new PlayerGround(sm, this));
        States.Add(PlayerStates.Jump, new PlayerJump(sm, this));
        States.Add(PlayerStates.Fall, new PlayerFall(sm, this));
        States.Add(PlayerStates.Move, new PlayerMove(sm, this));
        States.Add(PlayerStates.Idle, new PlayerIdle(sm, this));
    }
}
