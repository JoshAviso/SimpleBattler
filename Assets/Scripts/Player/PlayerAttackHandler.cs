using UnityEngine;

public class PlayerAttackHandler : AttackHandler
{
    public void AttackPerformed(AttackStatsScriptable attack)
    {
        Debug.Log($"Performed {attack.DisplayName}!");
    }
}
