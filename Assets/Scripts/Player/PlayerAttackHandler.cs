using UnityEngine;

public enum EAttackType
{
    None = -1, L = 0, LL = 1, LLL = 2, H = 3, LH = 4, LLH = 5, LLLH = 6, HH = 7
}

public class PlayerAttackHandler : AttackHandler
{
    public enum AttackInputType
    {
        None, Primary, Secondary, 
    }

    public void AttackPerformed(AttackStatsScriptable attack)
    {
        Debug.Log($"Performed {attack.DisplayName}!");
        PlayerStateHandler.PlayerState.PendingAction = EActionType.Attack;
        PlayerStateHandler.PlayerState.AttackType = attack.AttackType;
    }

    public void AttackInputDetected(AttackInputType inputType)
    {
        
    }
    
    // SINGLETON
    public static PlayerAttackHandler Instance { get; private set; }
    protected virtual void Awake(){
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }
    }
}
