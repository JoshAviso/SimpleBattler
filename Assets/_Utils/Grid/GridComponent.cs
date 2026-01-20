
using UnityEditor;
using UnityEngine;

public class GridComponent<TGridObject> : MonoBehaviour
{
    [SerializeField] protected Vector3Int _dimensions;
    [SerializeField] protected Vector3 _cellsize;
    [SerializeField] protected Grid<TGridObject> _grid;
    [SerializeField] protected bool _drawDebug = false;
    [SerializeField] protected Color _debugColor = Color.white;
    [SerializeField] protected Vector3 _cellLabelNormalizedOffset = Vector3.one * 0.5f;
    public static Vector3Int INVALID_CELL = Vector3Int.one * -1;

    public TGridObject GetObjectAtCell(uint x, uint y, uint z)
        { return _grid.GetObjectAtCell(x, y, z); }

    /// <returns>True if the grid address is within the grid's dimensions</returns>
    public bool SetObjectAt(uint x, uint y, uint z, TGridObject gridObject)
        { return _grid.SetObjectAt(x, y, z, gridObject); }
    public bool IsWithinBounds(uint x, uint y, uint z) { return _grid.IsWithinBounds(x, y, z); }
    public bool IsWithinBounds(int x, int y, int z) { return _grid.IsWithinBounds(x, y, z); }

    public Vector3 Origin => transform.position;

    /// <returns>True if the grid address is within the grid's dimensions</returns>
    public bool WorldToCell(Vector3 world, out uint x, out uint y, out uint z)
    {
        Vector3 relativeLoc = world - Origin;
        relativeLoc.x /= _cellsize.x;
        relativeLoc.y /= _cellsize.y;
        relativeLoc.z /= _cellsize.z;
        Vector3Int gridAddress = Vector3Int.FloorToInt(relativeLoc);
        x = (uint)gridAddress.x;
        y = (uint)gridAddress.y;
        z = (uint)gridAddress.z;

        return IsWithinBounds(gridAddress.x, gridAddress.y, gridAddress.z);
    }
    public Vector3 CellToWorld(uint x, uint y, uint z, Vector3 normPosWithinCell = new Vector3())
    {
        Vector3 offset = Vector3.Scale(normPosWithinCell, _cellsize);
        Vector3 worldOffset = new(x * _cellsize.x, y * _cellsize.y, z * _cellsize.z);

        return Origin + worldOffset + offset;
    }
    public Vector3 CellToWorld(int x, int y, int z, Vector3 normPosWithinCell = new Vector3())
        { return CellToWorld((uint)x, (uint)y, (uint)z, normPosWithinCell); }

    void Awake(){ 
        CheckDimensions();
    }

    protected void CheckDimensions()
    {
        if(_dimensions.x < 0 || _dimensions.y < 0 || _dimensions.z < 0) return;
        if((uint)_dimensions.x != _grid.XSize || (uint)_dimensions.y != _grid.YSize || (uint)_dimensions.z != _grid.ZSize)
            _grid.Resize((uint)_dimensions.x, (uint)_dimensions.y, (uint)_dimensions.z);
    }

    void OnDrawGizmos()
    {
        if(!_drawDebug) return;
        DrawLabels();
        DrawCellLines();
    }

    private void DrawLabels()
    {
        GUIStyle style = new();
        style.normal.textColor = _debugColor;

        for(int i = 0; i < _dimensions.x; i++)
        {
            for(int j = 0; j < _dimensions.y; j++)
            {
                for(int k = 0; k < _dimensions.z; k++)
                {
                    string label = GetCellLabel((uint)i, (uint)j, (uint)k);
                    Vector3 labelPos = CellToWorld((uint)i, (uint)j, (uint)k, _cellLabelNormalizedOffset);

                    Handles.Label(labelPos, label, style);
                }
            }
        }
    }
    protected virtual string GetCellLabel(uint x, uint y, uint z) { return $"({x}, {y}, {z})"; }
    
    private void DrawCellLines()
    {
        for(int i = 0; i < _dimensions.x; i++)
        {
            for(int j = 0; j < _dimensions.y; j++)
            {
                for(int k = 0; k < _dimensions.z; k++)
                {
                    Vector3 cellOrigin = CellToWorld((uint)i, (uint)j, (uint)k);

                    Vector3[] corners =
                    {
                        Vector3.Scale(new(1.0f, 0.0f, 0.0f), _cellsize) + cellOrigin,
                        Vector3.Scale(new(0.0f, 1.0f, 0.0f), _cellsize) + cellOrigin,
                        Vector3.Scale(new(0.0f, 0.0f, 1.0f), _cellsize) + cellOrigin,
                        Vector3.Scale(new(0.0f, 1.0f, 1.0f), _cellsize) + cellOrigin,
                        Vector3.Scale(new(1.0f, 0.0f, 1.0f), _cellsize) + cellOrigin,
                        Vector3.Scale(new(1.0f, 1.0f, 0.0f), _cellsize) + cellOrigin,
                        Vector3.Scale(new(1.0f, 1.0f, 1.0f), _cellsize) + cellOrigin,
                    };
                    
                    Debug.DrawLine(cellOrigin, corners[0], _debugColor);
                    Debug.DrawLine(cellOrigin, corners[1], _debugColor);
                    Debug.DrawLine(cellOrigin, corners[2], _debugColor);
                    Debug.DrawLine(corners[0], corners[4], _debugColor);
                    Debug.DrawLine(corners[0], corners[5], _debugColor);
                    Debug.DrawLine(corners[1], corners[3], _debugColor);
                    Debug.DrawLine(corners[1], corners[5], _debugColor);
                    Debug.DrawLine(corners[2], corners[3], _debugColor);
                    Debug.DrawLine(corners[2], corners[4], _debugColor);
                    Debug.DrawLine(corners[6], corners[3], _debugColor);
                    Debug.DrawLine(corners[6], corners[4], _debugColor);
                    Debug.DrawLine(corners[6], corners[5], _debugColor);
                }
            }
        }
    }
}