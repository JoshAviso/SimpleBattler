using System;
using UnityEngine;

public enum EActionType
{
    None,
    L_Atk, LL_Atk, LLL_Atk, H_Atk, HHold_Atk, LH_Atk, LLH_Atk, LLLH_Atk, 
    StanceSwitch_In, StanceSwitch_Out, 
    Perfect_Dodge, Dodge, Roll, Perfect_Parry, Block, 
    FullRun, 
}

public class PlayerActionDispatcher : MonoBehaviour
{
    EActionType _queuedAction;
    EActionType _currentAction;

    void Update()
    {
        if(CanExecute(EActionType.None))
            DispatchAction(_queuedAction);

    }

    void DispatchAction(EActionType action)
    {
        
    }

    bool CanExecute(EActionType action)
    {
        if(_currentAction != EActionType.None) 
            return false;
        return true;
    }

    public void ClearActionCallback()
    {
        _currentAction = EActionType.None;
    }
}
