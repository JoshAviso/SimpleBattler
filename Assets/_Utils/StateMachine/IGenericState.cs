using UnityEngine;
using System;

public abstract class IGenericState<EStateType, IContext> where EStateType : Enum where IContext : IStateMachineContext
{ 
    protected IContext _context;
    public IGenericState(EStateType state_type, IContext context) { StateType = state_type; _context = context; }

    public EStateType StateType { get; private set; }
    public virtual void OnEnterState(){}
    public virtual void OnExitState(){}
    public virtual void UpdateState(){}
    public virtual EStateType GetNextState(){ return StateType; }
    public virtual void OnTriggerEnter(Collider other){}
    public virtual void OnTriggerStay(Collider other){}
    public virtual void OnTriggerExit(Collider other){}
}
