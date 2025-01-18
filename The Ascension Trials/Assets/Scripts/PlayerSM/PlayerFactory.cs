/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates
{
    Basic,
}
public class PlayerFactory : Factory<PlayerStates, PlayerState, PlayerSM>
{
    Dictionary<PlayerStates, PlayerState> _States = new();
    public override Dictionary<PlayerStates, PlayerState> States => _States;

    public PlayerFactory(PlayerSM sm) : base(sm)
    {
        States.Add(PlayerStates.Basic, new PlayerIdle(sm, this));
    }
}
 */

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public enum PlayerStates
// {
//     Idle,
// }
// public class PlayerFactory : Factory<PlayerStates, PlayerState, PlayerSM>
// {
//     Dictionary<PlayerStates, PlayerState> _States = new();
//     public override Dictionary<PlayerStates, PlayerState> States => _States;

//     public PlayerFactory(PlayerSM sm) : base(sm)
//     {
//         States.Add(PlayerStates.Idle, new PlayerIdle(sm, this));
//     }
// }

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public enum PlayerStates
// {
//     Idle,
// }
// public class PlayerFactory : Factory<PlayerStates, PlayerState, PlayerSM>
// {
//     Dictionary<PlayerStates, PlayerState> _States = new();
//     public override Dictionary<PlayerStates, PlayerState> States => _States;

//     public PlayerFactory(PlayerSM sm) : base(sm)
//     {
//         States.Add(PlayerStates.Idle, new PlayerIdle(sm, this));
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates
{
    Idle,
}
public class PlayerFactory : Factory<PlayerStates, PlayerState, PlayerSM>
{
    Dictionary<PlayerStates, PlayerState> _States = new();
    public override Dictionary<PlayerStates, PlayerState> States => _States;

    public PlayerFactory(PlayerSM sm) : base(sm)
    {
        States.Add(PlayerStates.Idle, new PlayerIdle(sm, this));
    }
}
