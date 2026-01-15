
using UnityEngine;

public class PlayerPositionGrid : GridComponent<GameObject>
{
    private Vector3Int playerCell;
    void Update()
    {
        Vector3 playerPos = PlayerController.Instance.gameObject.transform.position;
        if(WorldToCell(playerPos, out var x, out var y, out var z))
            playerCell = new((int)x, (int)y, (int)z);
        else playerCell = new(-1, -1, -1);
    }

    protected override string GetCellLabel(uint x, uint y, uint z)
    {
        if(playerCell.x == (int)x && playerCell.y == (int)y && playerCell.z == (int)z)
            return "Player Cell";
        else return $"({x}, {y}, {z})";
    } 
}