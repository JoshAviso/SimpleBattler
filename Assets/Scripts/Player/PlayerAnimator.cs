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

        BodyFlags bodystate = PlayerStateHandler.PlayerState.BodyState;
        float alert = 1f;

        if(bodystate.HasFlag(BodyFlags.IsRunning | BodyFlags.IsAgile))
            alert = 4f;
        else if(bodystate.HasFlag(BodyFlags.IsRunning))
            alert = 3f;
        else if(bodystate.HasFlag(BodyFlags.IsAgile))
            alert = 2f;

        moveInput *= alert;

        _animator.SetFloat("XMove", moveInput.x);
        _animator.SetFloat("ZMove", moveInput.y);
        _animator.SetFloat("Alert", alert);
    
        _animator.SetBool("IsBlocking", PlayerStateHandler.PlayerState.BodyState.HasFlag(BodyFlags.IsBlocking));
        _animator.SetBool("IsGrounded", PlayerStateHandler.PlayerState.BodyState.HasFlag(BodyFlags.IsGrounded));
        UpdateAttackingState();

        _animator.SetBool("HasPendingMove", PlayerStateHandler.PlayerState.BodyState.HasFlag(BodyFlags.HasPendingMove));
        PlayerMoveHandler.MoveBeganPerformCallback();
        // _animator.SetInteger("AttackID", PlayerStateHandler.CurrentAttack ? PlayerStateHandler.CurrentAttack.AttackID : -1);
    }

    void UpdateAttackingState()
    {
        // _animator.SetBool("IsAttacking", PlayerStateHandler.PlayerState.BodyState.HasFlag(BodyFlags.IsAttacking));
    }
}
