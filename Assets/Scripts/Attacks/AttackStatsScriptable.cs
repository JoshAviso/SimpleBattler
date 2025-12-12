using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Stats", menuName = "Scriptables/Combat/Attacks/AttackStats", order = 0)]
public class AttackStatsScriptable : ScriptableObject
{
    public String DisplayName;
    public int AttackID;
}
