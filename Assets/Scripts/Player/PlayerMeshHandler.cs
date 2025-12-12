using System;
using UnityEngine;

public class PlayerMeshHandler : MonoBehaviour
{
    [Serializable] struct TurnSpeed { public float Normal, Slow, Fast, Instant; 
        public TurnSpeed(float n, float s, float f, float i){ Normal = n; Slow = s; Fast = f; Instant = i; } }

    [SerializeField] TurnSpeed _facingSpeeds = new (10f, 2f, 20f, 90f) ;

    void Update()
    {
        UpdateFacing(Time.deltaTime);
    }

    Vector3 _targetFacingTarget;
    public void BeginFaceTowards(Vector3 targetFacing)
    {
        targetFacing.y = 0;
        targetFacing.Normalize();
        _targetFacingTarget = targetFacing;
    }
    void UpdateFacing(float deltaTime)
    {
        if(_targetFacingTarget == Vector3.zero) return;

        float faceSpeed = 
            PlayerStateHandler.MoveState.HasFlag(MoveFlags.IsRunning) &&
            PlayerStateHandler.MoveState.HasFlag(MoveFlags.IsCrouching) ?
                _facingSpeeds.Normal :
            PlayerStateHandler.MoveState.HasFlag(MoveFlags.IsRunning) ?
                _facingSpeeds.Fast :
            PlayerStateHandler.MoveState.HasFlag(MoveFlags.IsCrouching) ?
                _facingSpeeds.Slow :
                _facingSpeeds.Normal;

        Quaternion rotation = Quaternion.LookRotation(_targetFacingTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, faceSpeed * deltaTime);
    }
}
