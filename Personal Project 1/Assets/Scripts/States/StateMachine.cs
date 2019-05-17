using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Scripts.States;
using System.Linq;

public class StateMachine : MonoBehaviour
{
    [SerializeField] public LayerMask _layerMask;

    private Dictionary<Type, BaseState> _availableStates;

    public BaseState CurrentState { get; set; }
    public event Action<BaseState> OnStateChanged;

    public void SetStates(Dictionary<Type, BaseState> states)
    {
        _availableStates = states;
    }

    void Update()
    {
        if(CurrentState == null)
        {
            CurrentState = _availableStates.Values.First();
        }

        var nextState = CurrentState?.Tick();

        if(nextState != null && nextState != CurrentState?.GetType())
        {
            CurrentState = _availableStates[nextState];
            OnStateChanged?.Invoke(CurrentState);
        }
    }

    private void SwitchToNewState(Type nextState)
    {
        CurrentState = _availableStates[nextState];
        OnStateChanged?.Invoke(CurrentState);
    }
}
