using UnityEngine;

public class FloorHandler : MonoBehaviour
{
    [SerializeField] Transform _checkLocation;
    [SerializeField] Vector3 _checkOffset;
    [SerializeField] LayerMask _groundMask = 1 << 3;
    [SerializeField] float _checkRadius = 0.2f;
    [SerializeField] float _checkDistance = 0.4f;
    [SerializeField] bool _showDebug;

    bool _isGrounded = false;
    public bool IsGrounded { get => _isGrounded; }

    void FixedUpdate()
    {
        UpdateGroundedState();
    }

    void OnDrawGizmos()
    {
        if(!_showDebug) return;

        DrawCheckRays();
    }

    void UpdateGroundedState()
    {
        Vector3 center = _checkLocation.position + _checkOffset;
        center.y += 0.01f;
        Vector3[] offsets = {
            Vector3.zero, 
            _checkLocation.right, -_checkLocation.right, 
            _checkLocation.forward, -_checkLocation.forward
        };

        foreach(Vector3 offset in offsets)
        {
            if (Physics.Raycast(center + (offset * _checkRadius), Vector3.down, _checkDistance, _groundMask))
            {
                _isGrounded = true;
                return;
            }
        }

        _isGrounded = false;
    }

    void DrawCheckRays()
    {
        Gizmos.color = _isGrounded ? Color.green : Color.red;

        Vector3 center = _checkLocation.position + _checkOffset;
        center.y += 0.01f;
        Vector3[] offsets = {
            Vector3.zero, 
            _checkLocation.right, -_checkLocation.right, 
            _checkLocation.forward, -_checkLocation.forward
        };

        foreach(Vector3 offset in offsets)
        {
            Gizmos.DrawRay(center + (offset * _checkRadius), Vector3.down * _checkDistance);
        }
    }
}
