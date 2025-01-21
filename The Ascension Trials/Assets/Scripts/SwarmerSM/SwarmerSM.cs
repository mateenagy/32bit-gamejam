using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmerSM : MonoBehaviour
{
    SwarmerState currentState;
    SwarmerFactory factory;

    #region GETTERS / SETTERS
    public SwarmerState CurrentState { get => currentState; set => currentState = value; }
    public SwarmerFactory Factory { get => factory; set => factory = value; }
    #endregion

    void Awake()
    {
        Factory = new SwarmerFactory(this);
        CurrentState = factory.States[SwarmerStates.Idle];
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
