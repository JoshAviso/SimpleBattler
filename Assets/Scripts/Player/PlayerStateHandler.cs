using System;
using UnityEngine;

[Flags] public enum MoveFlags {
    None = 0, 
    Running = 1 << 0, 
    Crouching = 1 << 1,
    IsGrounded = 1 << 2
}

public class PlayerStateHandler : MonoBehaviour
{
    public static Vector2 LookInput;
    public static Vector2 MoveInput;
    public static MoveFlags MoveState;

    [SerializeField] private MoveFlags _moveState;

    void Update()
    {
        _moveState = MoveState;
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
