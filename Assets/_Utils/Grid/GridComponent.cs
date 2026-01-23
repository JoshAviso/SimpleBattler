
using System;
using UnityEditor;
using UnityEngine;

public class GridComponent<TGridObject> : MonoBehaviour
{
    [SerializeField] protected Vector3Int _maxCoord;
    [SerializeField] protected Vector3Int _minCoord;
    [SerializeField] protected Vector3 _cellsize;
    [SerializeField] protected Grid<TGridObject> _grid;
    [SerializeField] protected bool _drawDebug = false;
    [SerializeField] protected Color _debugColor = Color.white;
    [SerializeField] protected Vector3 _cellLabelNormalizedOffset = Vector3.one * 0.5f;
    public static Vector3Int INVALID_CELL = Vector3Int.one * -99999;

    public bool GetObjectAtCell(int x, int y, int z, out TGridObject obj)
        { return _grid.GetObjectAtCell(x, y, z, out obj); }

    /// <returns>True if the grid address is within the grid's dimensions</returns>
    public bool SetObjectAt(int x, int y, int z, TGridObject gridObject)
        { return _grid.SetObjectAt(x, y, z, gridObject); }
    public bool IsWithinBounds(int x, int y, int z) { return _grid.IsWithinBounds(x, y, z); }

    public Vector3 Origin => transform.position;

    /// <returns>True if the grid address is within the grid's dimensions</returns>
    public bool WorldToCell(Vector3 world, out int x, out int y, out int z)
    {
        Vector3 relativeLoc = world - Origin;
        relativeLoc.x /= _cellsize.x;
        relativeLoc.y /= _cellsize.y;
        relativeLoc.z /= _cellsize.z;

        LogUtils.Log($"Relative Loc: {relativeLoc}");

        x = relativeLoc.x > 0 ? Mathf.FloorToInt(relativeLoc.x) : Mathf.CeilToInt(relativeLoc.x);
        y = relativeLoc.y > 0 ? Mathf.FloorToInt(relativeLoc.y) : Mathf.CeilToInt(relativeLoc.y);
        z = relativeLoc.z > 0 ? Mathf.FloorToInt(relativeLoc.z) : Mathf.CeilToInt(relativeLoc.z);

        LogUtils.Log($"Relative Loc Short: {x}, {y}, {z}");

        return IsWithinBounds(x, y, z);
    }
    public Vector3 CellToWorld(int x, int y, int z, Vector3 normPosWithinCell = new Vector3())
    {
        Vector3 offset = Vector3.Scale(normPosWithinCell, _cellsize);
        Vector3 worldOffset = new(x * _cellsize.x, y * _cellsize.y, z * _cellsize.z);

        return Origin + worldOffset + offset;
    }

    void Awake(){ 
        CheckDimensions();
    }

    protected void CheckDimensions()
    {
        if(
            _minCoord.x != _grid.Left || 
            _minCoord.x != _grid.Right || 
            _minCoord.y != _grid.Bottom || 
            _minCoord.y != _grid.Top || 
            _minCoord.z != _grid.Back || 
            _minCoord.z != _grid.Front 
        ) 
            _grid.Resize(_maxCoord.x, _maxCoord.y, _maxCoord.z, _minCoord.x, _minCoord.y, _minCoord.z);
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

        for(int i = _minCoord.x; i <= _maxCoord.x; i++)
        {
            for(int j = _minCoord.y; j <= _maxCoord.y; j++)
            {
                for(int k = _minCoord.z; k <= _maxCoord.z; k++)
                {
                    string label = GetCellLabel(i, j, k);
                    Vector3 labelPos = CellToWorld(i, j, k, _cellLabelNormalizedOffset);

                    Handles.Label(labelPos, label, style);
                }
            }
        }
    }
    protected virtual string GetCellLabel(int x, int y, int z) { return $"({x}, {y}, {z})"; }
    
    private void DrawCellLines()
    {
        for(int i = _minCoord.x; i <= _maxCoord.x; i++)
        {
            for(int j = _minCoord.y; j <= _maxCoord.y; j++)
            {
                for(int k = _minCoord.z; k <= _maxCoord.z; k++)
                {
                    Vector3 cellOrigin = CellToWorld(i, j, k);

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