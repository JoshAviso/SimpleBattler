using System;
using UnityEngine;

[Flags] public enum MoveFlags {
    None = 0, 
    IsRunning = 1 << 0, 
    IsCrouching = 1 << 1,
    IsGrounded = 1 << 2,
    IsAttacking = 1 << 3
}

public class PlayerStateHandler : MonoBehaviour
{
    public static Vector2 LookInput;
    public static Vector2 MoveInput;
    public static MoveFlags MoveState;
    public static AttackStatsScriptable CurrentAttack;

    [SerializeField] private MoveFlags _moveState;
    void Update()
    {
        _moveState = MoveState;
        MoveState |= MoveFlags.IsGrounded; // Temp, need to do ground checking
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
