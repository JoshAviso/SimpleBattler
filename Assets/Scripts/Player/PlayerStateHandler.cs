using System;
using UnityEngine;

[Flags] public enum BodyFlags {
    None = 0, 
    IsRunning = 1 << 0, 
    IsBlocking = 1 << 1,
    IsAgile = 1 << 2,
    IsGrounded = 1 << 3,
    IsAttacking = 1 << 4,
}

[Serializable] public struct PlayerState
{
    public Vector2 MoveInput;
    public BodyFlags BodyState;
    public EActionType Action;
}


public class PlayerStateHandler : MonoBehaviour
{
    public static Vector2 LookInput;
    public static PlayerState PlayerState;
    public static PlayerState Previous_PlayerState;

    [SerializeField] private BodyFlags _actionState;

    private FloorHandler _floorCheck;
    void Start()
    {
        _floorCheck = GetComponentInChildren<FloorHandler>();
    }

    void Update()
    {
        UpdateIsGrounded();
        _actionState = PlayerState.BodyState;
    }

    void UpdateIsGrounded()
    {
        if (_floorCheck)
        {
            if(_floorCheck.IsGrounded)
                PlayerState.BodyState |= BodyFlags.IsGrounded;
            else
                PlayerState.BodyState &= ~BodyFlags.IsGrounded;
        }
        // If cant find floor check, assume player is always on floor
        else PlayerState.BodyState |= BodyFlags.IsGrounded;
    }

    public static int CurrentAttackID => GetAttackID(PlayerState);
    public static int PreviousAttackID => GetAttackID(Previous_PlayerState);
    static int GetAttackID(PlayerState state)
    {
        int id = state.Action switch
        {
            _ => -1
        };
        if (id <= -1) return -1;

        return 1;
    }


    // SINGLETON
    public static PlayerStateHandler Instance { get; private set; }
    protected virtual void Awake(){
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }
        
    }
}
