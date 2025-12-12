using UnityEngine;
using UnityEngine.Events;

public class PlayerAttackHandler : AttackHandler
{
    public UnityEvent _onAttackFinished;

    public void AttackPerformed(AttackStatsScriptable attack)
    {
        Debug.Log($"Performed {attack.DisplayName}!");
        PlayerStateHandler.CurrentAttack = attack;
        PlayerStateHandler.MoveState |= MoveFlags.IsAttacking;
    }
    public void AttackFinishedCallback()
    {
        PlayerStateHandler.MoveState &= ~MoveFlags.IsAttacking;
        LogUtils.Log("Received attack callback");
        _onAttackFinished?.Invoke();
    }
}
