
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform _lookTransform;
    [SerializeField] PlayerMeshHandler _meshHandler;

    [Serializable] struct MoveSpeed { public float Normal, Crouching, Running, CrouchRun; 
        public MoveSpeed(float n, float c, float r, float cr){ Normal = n; Crouching = c; Running = r; CrouchRun = cr; } }

    [SerializeField] MoveSpeed _moveSpeeds = new () ;

    Rigidbody _rb;

    void Awake() 
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move(Time.fixedDeltaTime);
    }

    public void Move(float deltaTime)
    {
        Vector2 normalizedInput = PlayerStateHandler.MoveInput;

        if(normalizedInput.sqrMagnitude > 1.0f)
            normalizedInput.Normalize();

        Transform lookTransform = _lookTransform ? _lookTransform : transform;
        Vector3 right = lookTransform.right * normalizedInput.x;
        Vector3 flatForward = lookTransform.forward;
        flatForward.y = 0.0f;
        flatForward.Normalize();

        Vector3 moveDir = flatForward * normalizedInput.y + right;
        moveDir.Normalize();

        float moveSpeed = 
            PlayerStateHandler.MoveState.HasFlag(MoveFlags.Running) &&
            PlayerStateHandler.MoveState.HasFlag(MoveFlags.Crouching) ?
                _moveSpeeds.CrouchRun :
            PlayerStateHandler.MoveState.HasFlag(MoveFlags.Running) ?
                _moveSpeeds.Running :
            PlayerStateHandler.MoveState.HasFlag(MoveFlags.Crouching) ?
                _moveSpeeds.Crouching :
                _moveSpeeds.Normal;

        Vector3 move3D = moveSpeed * deltaTime * moveDir;

        // Apply movement speed
        float yVelocity = _rb.linearVelocity.y;
        _rb.linearVelocity = new(move3D.x, yVelocity, move3D.z);

        // Update Mesh Updater
        if(_meshHandler && normalizedInput.sqrMagnitude != 0.0f)
            _meshHandler.BeginFaceTowards(flatForward);
    }

}
