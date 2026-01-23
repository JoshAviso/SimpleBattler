
using System;
using UnityEngine;

[Flags] public enum EGridRotation
{
    None = 0, 
    CW = 1 << 0, 
    CCW = 1 << 1, 
    Double = CW | CCW, 
    Mirrored = 1 << 2
}

public class ObjectPlacementGrid : GridComponent<GridGameObject>
{
    private Vector3Int playerCell;
    public Vector3Int PlacementPos => playerCell;
    [SerializeField] private GameObject _buildIndicator;
    [SerializeField] private float _lookforwardDist;

    void Update()
    {
        UpdatePlayerPosition();
        UpdateBuildIndicator();
    }

    public bool TrySetObject(GridGameObject obj, int x, int y, int z, EGridRotation rotation = EGridRotation.None)
    {
        Vector3Int bounds = obj.GridDimensions;
        if(bounds.x < 0 || bounds.y < 0 || bounds.z < 0)
        {
            LogUtils.LogWarning($"Trying to place invalid object ${obj} with size ${bounds}");
            return false;
        }

        // Clear mirror byte as irrelevant
        rotation &= ~EGridRotation.Mirrored;
        // Switch x and z for 90deg rotated
        if(rotation.Equals(EGridRotation.CW) || rotation.Equals(EGridRotation.CCW))
            (bounds.z, bounds.x) = (bounds.x, bounds.z);

        // Change 'bottom left' check origin based on rotation
        if(rotation.HasFlag(EGridRotation.CW)) z -= bounds.z - 1;
        if(rotation.HasFlag(EGridRotation.CCW)) x -= bounds.x - 1;

        for (int i = 0; i < bounds.x; i++)
        for (int j = 0; j < bounds.y; j++)
        for (int k = 0; k < bounds.z; k++)
        {
            // LogUtils.Log($"Object at cell: {x+i}, {y+j}, {z+k}, {GetObjectAtCell(x + i, y + j, z + k, out _)}");
            if(!IsWithinBounds(x + i, y + j, z + k)) return false;
            if(GetObjectAtCell(x + i, y + j, z + k, out _)) return false;
        }

        for (int i = 0; i < bounds.x; i++)
        for (int j = 0; j < bounds.y; j++)
        for (int k = 0; k < bounds.z; k++)
            SetObjectAt(x + i, y + j, z + k, obj);

        return true;
    }

    protected override string GetCellLabel(int x, int y, int z)
    {
        if(_grid.GetObjectAtCell(x, y, z, out var obj))
            return obj.name;
        else return $"({x}, {y}, {z})";
    } 

    private void UpdatePlayerPosition()
    {
        Vector3 playerPos = PlayerController.Instance.transform.GetChild(0).position;
        float origY = playerPos.y;
        Vector3 camForward = PlayerController.Instance.transform.GetChild(0).GetChild(0).forward;
        Vector3 lookAhead = Vector3.Scale(0.5f * _cellsize + Vector3.one * _lookforwardDist, camForward);
        playerPos += lookAhead;
        playerPos.y = origY;

        // LogUtils.Log($"In bounds: {WorldToCell(playerPos, out var h, out var k, out var l)}, {h}, {k}, {l}. {_grid.Left}, {_grid.Right}, {_grid.Bottom}, {_grid.Top}, {_grid.Front}, {_grid.Back}");

        if(WorldToCell(playerPos, out var x, out var y, out var z))
            playerCell = new(x, y, z);
        else playerCell = INVALID_CELL;
    }

    private void UpdateBuildIndicator()
    {
        if(playerCell == INVALID_CELL){
            _buildIndicator.SetActive(false);
            return;
        }
        
        _buildIndicator.SetActive(true);
        _buildIndicator.transform.position = CellToWorld(playerCell.x, playerCell.y, playerCell.z, new(0.5f, 0f, 0.5f));
    }
    
    // SINGLETON
    public static ObjectPlacementGrid Instance { get; private set; }
    protected virtual void Awake(){
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);

            CheckDimensions();
        } else {
            Destroy(gameObject);
        }
    }
}