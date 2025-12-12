using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool InputEnabled = true;
    [SerializeReference] InputActionAsset _inputMap;

    [Header("Movement")]
    [SerializeReference] InputActionReference _moveAction;
    [SerializeReference] InputActionReference _lookAction;
    [SerializeReference] InputActionReference _runAction;
    [SerializeField] bool _toggleRun = false;
    [SerializeReference] InputActionReference _crouchAction;
    [SerializeField] bool _toggleCrouch = false;

    [Serializable] public struct AttackInputMapping { 
        [SerializeReference] public InputActionReference AttackAction; 
        [SerializeReference] public AttackStatsScriptable AttackRef; }

    [Header("Combat")]
    [SerializeField] List<AttackInputMapping> _attackMappings = new();

    void Start()
    {
        _inputMap?.Enable(); 
        SetCursorStatus(ECursorStatus.Locked);

        _attackHandler = GetComponentInChildren<PlayerAttackHandler>();
        
    }

    void Update()
    {
        ProcessMoveInput();
        ProcessLookInput();
        ProcessMoveTypeUpdate();
        ProcessAttackInput();
    }

    public enum ECursorStatus {
        Unlocked, Locked, Limited
    }
    public void SetCursorStatus(ECursorStatus status)
    {
        switch(status)
        {
            case ECursorStatus.Unlocked:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case ECursorStatus.Locked: 
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case ECursorStatus.Limited:
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                break;
        }
    }

#region Movement Processing
    void ProcessMoveInput()
    {
        PlayerStateHandler.MoveInput = Vector2.zero;

        if(!InputEnabled) return;
        if(_moveAction == null) return;

        PlayerStateHandler.MoveInput = _moveAction.action.ReadValue<Vector2>();
        if(PlayerStateHandler.MoveInput.sqrMagnitude > 1.0f)
            PlayerStateHandler.MoveInput.Normalize();
    }

    void ProcessMoveTypeUpdate()
    {
        if(!InputEnabled) return;
        
        bool isCrouchPressed = false; 
        bool crouchWasPressed = false;
        bool isRunPressed = false;
        bool runWasPressed = false;

        if(_crouchAction != null) 
        {
            isCrouchPressed = _crouchAction.action.IsPressed();
            crouchWasPressed = _crouchAction.action.WasPerformedThisFrame();
        }
        
        if(_runAction != null)
        {
            isRunPressed = _runAction.action.IsPressed();
            runWasPressed = _runAction.action.WasPerformedThisFrame();
        }

        if(!_toggleCrouch)
        {
            if(isCrouchPressed)
                PlayerStateHandler.MoveState |= MoveFlags.IsCrouching;
            else PlayerStateHandler.MoveState &= ~MoveFlags.IsCrouching;
        }
        else if(crouchWasPressed)
        {   
            if(PlayerStateHandler.MoveState.HasFlag(MoveFlags.IsCrouching))
                PlayerStateHandler.MoveState &= ~MoveFlags.IsCrouching;
            else PlayerStateHandler.MoveState |= MoveFlags.IsCrouching;
        }

        if(!_toggleRun)
        {   
            if(isRunPressed)
                PlayerStateHandler.MoveState |= MoveFlags.IsRunning;
            else PlayerStateHandler.MoveState &= ~MoveFlags.IsRunning;
        }
        else if(runWasPressed)
        {   
            if(PlayerStateHandler.MoveState.HasFlag(MoveFlags.IsRunning))
                PlayerStateHandler.MoveState &= ~MoveFlags.IsRunning;
            else PlayerStateHandler.MoveState |= MoveFlags.IsRunning;
        }
    }
#endregion
    
    void ProcessLookInput()
    {
        PlayerStateHandler.LookInput = Vector2.zero;
        
        if(!InputEnabled) return;
        if(_lookAction == null) return;

        PlayerStateHandler.LookInput = _lookAction.action.ReadValue<Vector2>();
        if(PlayerStateHandler.LookInput.sqrMagnitude > 1.0f)
            PlayerStateHandler.LookInput.Normalize();
    }    

    PlayerAttackHandler _attackHandler; 
    void ProcessAttackInput()
    {
        if(!InputEnabled) return;
        if(!_attackHandler) return;

        foreach (AttackInputMapping attack in _attackMappings)
        {
            if(attack.AttackAction == null) continue;
            if(attack.AttackRef == null) continue;

            if(attack.AttackAction.action.WasPerformedThisFrame())
                _attackHandler.AttackPerformed(attack.AttackRef);
        }
    }
    
    // SINGLETON
    public static PlayerController Instance { get; private set; }
    protected virtual void Awake(){
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }
    }
}
