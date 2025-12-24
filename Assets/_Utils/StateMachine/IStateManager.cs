using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class IStateManager<EStateType, IContext> : MonoBehaviour where EStateType : Enum where IContext : IStateMachineContext
{
    protected Dictionary<EStateType, IGenericState<EStateType, IContext>> _stateList = new();
    protected IGenericState<EStateType, IContext> _currentState;
    bool _isTransitioningState = false;

    [SerializeField] protected IContext _context;

    void Start() { 
        _isTransitioningState = true;
        _currentState.OnEnterState(); 
        _isTransitioningState = false; 
    }

    void Update() {
        if(_isTransitioningState) return;

        EStateType nextState = _currentState.GetNextState();

        if(nextState.Equals(_currentState.StateType))
            _currentState.UpdateState();
        else TransitionToState(nextState);
    }

    public void TransitionToState(EStateType state_key)
    {
        if(!_stateList.ContainsKey(state_key))
        {
            LogUtils.LogError(this, $"Trying to transition to uninitialized state, {state_key}");
            return;
        }

        _isTransitioningState = true;
        _currentState.OnExitState();
        _currentState = _stateList[state_key];
        _currentState.OnEnterState();
        _isTransitioningState = false;
    }

    protected void RegisterState(IGenericState<EStateType, IContext> state)
    {
        _stateList.Add(state.StateType, state);
    }

    void OnTriggerEnter(Collider other){ _currentState.OnTriggerEnter(other); }
    void OnTriggerStay(Collider other){ _currentState.OnTriggerStay(other); }
    void OnTriggerExit(Collider other){ _currentState.OnTriggerExit(other); }
}
