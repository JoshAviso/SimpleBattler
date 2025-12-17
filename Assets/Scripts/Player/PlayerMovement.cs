
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform _lookTransform;
    [SerializeField] PlayerMeshHandler _meshHandler;

    [Serializable] struct MoveSpeed { public float Normal, Blocking, QuickBlock, Jogging, Running, Sprinting, Aerial; 
        public MoveSpeed(float n, float b, float qb, float j, float r, float s, float a){ Normal = n; Blocking = b; QuickBlock = qb; Jogging = j; Running = r; Sprinting = s; Aerial = a;} }

    [SerializeField] MoveSpeed _moveSpeeds = new();

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
        Vector2 normalizedInput = PlayerStateHandler.PlayerState.MoveInput;

        if(normalizedInput.sqrMagnitude > 1.0f)
            normalizedInput.Normalize();

        Transform lookTransform = _lookTransform ? _lookTransform : transform;
        Vector3 right = lookTransform.right * normalizedInput.x;
        Vector3 flatForward = lookTransform.forward;
        flatForward.y = 0.0f;
        flatForward.Normalize();

        Vector3 moveDir = flatForward * normalizedInput.y + right;
        moveDir.Normalize();

        BodyFlags bodystate = PlayerStateHandler.PlayerState.BodyState;

        float moveSpeed = 
            !bodystate.HasFlag(BodyFlags.IsGrounded) ?
                _moveSpeeds.Aerial :
            bodystate.HasFlag(BodyFlags.IsBlocking) && (bodystate.HasFlag(BodyFlags.IsAgile) || bodystate.HasFlag(BodyFlags.IsRunning)) ?
                _moveSpeeds.QuickBlock :
            bodystate.HasFlag(BodyFlags.IsAgile | BodyFlags.IsRunning) ?
                _moveSpeeds.Sprinting :
            bodystate.HasFlag(BodyFlags.IsBlocking) ?
                _moveSpeeds.Blocking :
            bodystate.HasFlag(BodyFlags.IsAgile) ?
                _moveSpeeds.Jogging :
            bodystate.HasFlag(BodyFlags.IsRunning) ?
                _moveSpeeds.Running :
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
