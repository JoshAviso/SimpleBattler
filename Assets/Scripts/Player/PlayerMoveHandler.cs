using UnityEngine;

public class PlayerMoveHandler : AttackHandler
{
    public enum AttackInputType
    {
        None, Primary, Secondary, 
    }

    public void AttackPerformed(AttackStatsScriptable attack)
    {
        Debug.Log($"Performed {attack.DisplayName}!");
        PlayerStateHandler.PlayerState.BodyState |= BodyFlags.HasPendingMove;
    }

    static public void MoveBeganPerformCallback()
    {
        PlayerStateHandler.PlayerState.BodyState &= ~BodyFlags.HasPendingMove;
        
    }
    
    // SINGLETON
    public static PlayerMoveHandler Instance { get; private set; }
    protected virtual void Awake(){
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }
    }
}
