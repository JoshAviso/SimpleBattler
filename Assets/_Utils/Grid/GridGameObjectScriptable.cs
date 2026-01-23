using UnityEngine;

[CreateAssetMenu(fileName = "New GridGameObject", menuName = "Scriptables/Grid/GridGameObject", order = 0)]
public class GridGameObject : ScriptableObject
{
    public GameObject Prefab;
    public Vector3Int GridDimensions = Vector3Int.one;

}
