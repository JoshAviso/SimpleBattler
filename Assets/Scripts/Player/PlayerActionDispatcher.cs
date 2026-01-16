using System;
using UnityEngine;


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
