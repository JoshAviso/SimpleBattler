using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    Animator _animator;
    void Awake()
    {
        _animator = GetComponent<Animator>();
        if (!_animator)
        {
            LogUtils.LogWarning(this, "Could not find Animator component");
            Destroy(this);
        }
    }
    void Update()
    {
        UpdateAnimState();
    }
    void UpdateAnimState()
    {
        Vector2 moveInput = PlayerStateHandler.MoveInput;
        if(moveInput.sqrMagnitude > 1f) moveInput.Normalize();
        _animator.SetFloat("XMove", moveInput.x);
        _animator.SetFloat("YMove", moveInput.y);
    
        _animator.SetBool("IsMoving", PlayerStateHandler.MoveInput.sqrMagnitude > 0f);
        _animator.SetBool("IsRunning", PlayerStateHandler.MoveState.HasFlag(MoveFlags.IsRunning));
        _animator.SetBool("IsCrouching", PlayerStateHandler.MoveState.HasFlag(MoveFlags.IsCrouching));
        _animator.SetBool("IsGrounded", PlayerStateHandler.MoveState.HasFlag(MoveFlags.IsGrounded));
        UpdateAttackingState();

        _animator.SetInteger("AttackID", PlayerStateHandler.CurrentAttack ? PlayerStateHandler.CurrentAttack.AttackID : -1);
    }

    void UpdateAttackingState()
    {
        _animator.SetBool("IsAttacking", PlayerStateHandler.MoveState.HasFlag(MoveFlags.IsAttacking));
    }
}
