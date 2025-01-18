using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySM : MonoBehaviour
{
    EnemyState currentState;
    EnemyFactory factory;

    #region GETTERS / SETTERS
    public EnemyState CurrentState { get => currentState; set => currentState = value; }
    public EnemyFactory Factory { get => factory; set => factory = value; }
    #endregion

    void Awake()
    {
        Factory = new EnemyFactory(this);
        CurrentState = factory.States[EnemyStates.Basic];
    }

    void Start()
    {
        CurrentState.EnterStates();
    }

    void Update()
    {
        CurrentState.UpdateStates();
    }
}
