using UnityEngine;
using UnityEngine.Events;

public class PlayerAttackHandler : AttackHandler
{
    public UnityEvent _onAttackFinished;

    public void AttackPerformed(AttackStatsScriptable attack)
    {
        Debug.Log($"Performed {attack.DisplayName}!");
        // PlayerStateHandler.PlayerState.CurrentAttack = attack;
        PlayerStateHandler.PlayerState.BodyState |= BodyFlags.IsAttacking;
    }
    public void AttackFinishedCallback()
    {
        PlayerStateHandler.PlayerState.BodyState &= ~BodyFlags.IsAttacking;
        // LogUtils.Log("Received attack callback");
        _onAttackFinished?.Invoke();
    }
}
