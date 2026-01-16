using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool InputEnabled = true;
    [SerializeReference] InputActionAsset _inputMap;

    [Header("Basic Controls")]
    [SerializeReference] InputActionReference _moveAction;
    [SerializeReference] InputActionReference _lookAction;
    [SerializeReference] InputActionReference _runAction;
    [SerializeReference] InputActionReference _agileAction;

    [Serializable] public struct AttackInputMapping { 
        [SerializeReference] public InputActionReference AttackAction; 
        [SerializeReference] public AttackStatsScriptable AttackRef; }

    [Serializable] public struct ActionInputMapping
    {
        [SerializeReference] public InputActionReference InputAction;
        public UnityEvent OnPress;
        public UnityEvent OnPerformed;
        public UnityEvent OnRelease;
        public UnityEvent OnCompleted;
    }

    [Header("Combat")]
    [SerializeReference] InputActionReference _primaryAttackAction;
    [SerializeReference] InputActionReference _secondaryAttackAction;
    [SerializeReference] InputActionReference _throwAction;
    [SerializeReference] InputActionReference _defendAction;
    private PlayerAttackHandler _attackHandler;
    [SerializeField] List<ActionInputMapping> _actionMappings = new();

    void Start()
    {
        _inputMap?.Enable(); 
        SetCursorStatus(ECursorStatus.Locked);
        _attackHandler = GetComponent<PlayerAttackHandler>();
    }

    void Update()
    {
        ProcessMoveInput();
        ProcessLookInput();

        ProcessBodyStateInputs();
     
        ProcessActionInputs();
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

    void ProcessMoveInput()
    {
        PlayerStateHandler.PlayerState.MoveInput = Vector2.zero;

        if(!InputEnabled) return;
        if(_moveAction == null) return;

        PlayerStateHandler.PlayerState.MoveInput = _moveAction.action.ReadValue<Vector2>();
        if(PlayerStateHandler.PlayerState.MoveInput.sqrMagnitude > 1.0f)
            PlayerStateHandler.PlayerState.MoveInput.Normalize();
    }

    void ProcessBodyStateInputs()
    {
        if(!InputEnabled) return;
        
        bool isBlockPressed = false; 
        bool isRunPressed = false;
        bool isAgilePressed = false;

        if(_defendAction != null) 
        {
            isBlockPressed = _defendAction.action.IsPressed();
        }
        
        if(_runAction != null)
        {
            isRunPressed = _runAction.action.IsPressed();
        }

        if(_agileAction != null)
        {
            isAgilePressed = _agileAction.action.IsPressed();
        }

        if(isBlockPressed)
            PlayerStateHandler.PlayerState.BodyState |= BodyFlags.IsBlocking;
        else PlayerStateHandler.PlayerState.BodyState &= ~BodyFlags.IsBlocking;

        if(isRunPressed)
            PlayerStateHandler.PlayerState.BodyState |= BodyFlags.IsRunning;
        else PlayerStateHandler.PlayerState.BodyState &= ~BodyFlags.IsRunning;

        if(isAgilePressed)
            PlayerStateHandler.PlayerState.BodyState |= BodyFlags.IsAgile;
        else PlayerStateHandler.PlayerState.BodyState &= ~BodyFlags.IsAgile;
    }
    
    void ProcessLookInput()
    {
        PlayerStateHandler.LookInput = Vector2.zero;
        
        if(!InputEnabled) return;
        if(_lookAction == null) return;

        PlayerStateHandler.LookInput = _lookAction.action.ReadValue<Vector2>();
        if(PlayerStateHandler.LookInput.sqrMagnitude > 1.0f)
            PlayerStateHandler.LookInput.Normalize();
    }    

    void ProcessActionInputs()
    {
        if(!InputEnabled) return;

        if(_primaryAttackAction && _primaryAttackAction.action.WasPerformedThisFrame())
            LogUtils.Log(this, "Primary Atk Input");
        if(_secondaryAttackAction && _secondaryAttackAction.action.WasPerformedThisFrame())
            LogUtils.Log(this, "Secondary Atk Input");
        // _hand

        foreach(ActionInputMapping action in _actionMappings)
        {
            if(action.InputAction == null) continue;
            
            if(action.InputAction.action.WasPressedThisFrame())
                action.OnPress?.Invoke();
            if(action.InputAction.action.WasPerformedThisFrame())
                action.OnPerformed?.Invoke();
            if(action.InputAction.action.WasReleasedThisFrame())
                action.OnRelease?.Invoke();
            if(action.InputAction.action.WasCompletedThisFrame())
                action.OnCompleted?.Invoke();
        }
    }

    public void PrimaryAtk()
    {
        LogUtils.Log("Attack!");
    }
    public void Throw()
    {
        LogUtils.Log("Throw!");
    }
    public void Interact()
    {
        LogUtils.Log("Interact!");
    }
    public void Dodge()
    {
        LogUtils.Log("Dodge!");
    }
    public void Roll()
    {
        LogUtils.Log("Roll!");
    }
    public void StartBlock()
    {
        LogUtils.Log("Start Block!");
    }
    public void SecondaryAtkPress()
    {
        LogUtils.Log("Secondary Press!");
    }
    public void SecondaryAtkRelease()
    {
        LogUtils.Log("Secondary Release!");
    }
    public void ToggleSkillMenu()
    {
        LogUtils.Log("Toggle Skill Menu!");
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
