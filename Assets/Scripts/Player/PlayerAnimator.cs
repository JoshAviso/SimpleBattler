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
        Vector2 moveInput = PlayerStateHandler.PlayerState.MoveInput;
        if(moveInput.sqrMagnitude > 1f) moveInput.Normalize();
        _animator.SetFloat("XMove", moveInput.x);
        _animator.SetFloat("YMove", moveInput.y);
    
        _animator.SetBool("IsMoving", PlayerStateHandler.PlayerState.MoveInput.sqrMagnitude > 0f);
        _animator.SetBool("IsRunning", PlayerStateHandler.PlayerState.BodyState.HasFlag(BodyFlags.IsRunning));
        _animator.SetBool("IsBlocking", PlayerStateHandler.PlayerState.BodyState.HasFlag(BodyFlags.IsBlocking));
        _animator.SetBool("IsAgile", PlayerStateHandler.PlayerState.BodyState.HasFlag(BodyFlags.IsAgile));
        _animator.SetBool("IsGrounded", PlayerStateHandler.PlayerState.BodyState.HasFlag(BodyFlags.IsGrounded));
        UpdateAttackingState();

        _animator.SetBool("IsAttacking", PlayerStateHandler.PlayerState.BodyState.HasFlag(BodyFlags.IsAttacking));
        // _animator.SetInteger("AttackID", PlayerStateHandler.CurrentAttack ? PlayerStateHandler.CurrentAttack.AttackID : -1);
    }

    void UpdateAttackingState()
    {
        // _animator.SetBool("IsAttacking", PlayerStateHandler.PlayerState.BodyState.HasFlag(BodyFlags.IsAttacking));
    }
}
