
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] float _horizontalLookSpeed;
    [SerializeField] float _verticalLookSpeed;
    [SerializeField] Vector2 _verticalLookBounds;

    void Update()
    {
        UpdateLook(Time.deltaTime);
    }

    float _trackedPitch = 0.0f; float _trackedYaw = 0.0f;
    public void UpdateLook(float deltaTime)
    {
        Vector2 normalizedInput = PlayerStateHandler.LookInput;

        if(normalizedInput.sqrMagnitude > 1.0f) 
            normalizedInput.Normalize();

        float verticalLook = _verticalLookSpeed * -normalizedInput.y * deltaTime;
        _trackedPitch += verticalLook;
        _trackedPitch = Mathf.Clamp(_trackedPitch, _verticalLookBounds.x, _verticalLookBounds.y);

        float horizontalLook = _horizontalLookSpeed * normalizedInput.x * deltaTime;
        _trackedYaw += horizontalLook;
        if(Mathf.Abs(_trackedYaw) > 180.0f)
            _trackedYaw += 360.0f * -Mathf.Sign(_trackedYaw);
        
        transform.localRotation = Quaternion.Euler(_trackedPitch, _trackedYaw, 0.0f);
    }
}
