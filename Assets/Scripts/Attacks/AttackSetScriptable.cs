
using System;
using System.Collections.Generic;
using UnityEngine;

public enum EActionVariant
{
    L_Default, LL_Default, LLL_Default, H_Default, HH_Default, HHH_Default,
    L_Upgrade, LL_Upgrade, LLL_Upgrade, H_Upgrade, HH_Upgrade, HHH_Upgrade,
    LH_Default, LLH_Default, LLLH_Default, LH_Upgrade, LLH_Upgrade, LLLH_Upgrade,
    Grab_Default, Grab_Upgrade, Counter_Default, Counter_Upgrade
}

[CreateAssetMenu(fileName = "New AttackSetScriptable", menuName = "Scriptables/Combat/Attacks/AttackSet", order = 0)]
public class AttackSetScriptable : ScriptableObject
{
    [SerializeReference] AnimatorOverrideController _overrideAnimController;
    [Serializable] struct AttackStatsDirectionList
    {
        [SerializeReference] AttackStatsScriptable _neutral;
        [SerializeReference] AttackStatsScriptable _forward;
        [SerializeReference] AttackStatsScriptable _backward;
        [SerializeReference] AttackStatsScriptable _left;
        [SerializeReference] AttackStatsScriptable _right;
        [SerializeReference] AttackStatsScriptable _snap_forward;
        [SerializeReference] AttackStatsScriptable _snap_backwards;
        [SerializeReference] AttackStatsScriptable _snap_left;
        [SerializeReference] AttackStatsScriptable _snap_right;
    }

    [Header("Attack Stats")]
    AttackStatsDirectionList _light1_default;
    AttackStatsDirectionList _light2_default;
    AttackStatsDirectionList _light3_default;
    AttackStatsDirectionList _heavy1_default;
    AttackStatsDirectionList _heavy2_default;
    AttackStatsDirectionList _heavy3_default;
    AttackStatsDirectionList _light1_upgrade;
    AttackStatsDirectionList _light2_upgrade;
    AttackStatsDirectionList _light3_upgrade;
    AttackStatsDirectionList _heavy1_upgrade;
    AttackStatsDirectionList _heavy2_upgrade;
    AttackStatsDirectionList _heavy3_upgrade;
    
}
