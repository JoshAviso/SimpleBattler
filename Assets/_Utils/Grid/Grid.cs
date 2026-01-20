
using System;
using UnityEngine;

public interface IGridObject
{
    public uint XSize { get; }
    public uint YSize { get; }
    public uint ZSize { get; }
}
[Serializable] public class Grid<TGridObject>
{
    public uint XSize { get; private set; }
    public uint YSize { get; private set; }
    public uint ZSize { get; private set; }
    public TGridObject[,,] _objects;
    public void Resize(uint newX, uint newY, uint newZ)
    {

        if (_objects == null)
        {
            XSize = newX; 
            YSize = newY; 
            ZSize = newZ;
            _objects = new TGridObject[newX, newY, newZ];
            return;
        }

        TGridObject[,,] newArray = new TGridObject[newX, newY, newZ];

        uint minX = Math.Min(XSize, newX);
        uint minY = Math.Min(YSize, newY);
        uint minZ = Math.Min(ZSize, newZ);

        XSize = newX; 
        YSize = newY; 
        ZSize = newZ;
        
        for (uint x = 0; x < minX; ++x)
        for (uint y = 0; y < minY; ++y)
        for (uint z = 0; z < minZ; ++z)
            newArray[x, y, z] = _objects[x, y, z];

        _objects = newArray;
    }

    public TGridObject GetObjectAtCell(uint x, uint y, uint z)
        { return IsWithinBounds(x, y, z) ? _objects[x, y, z] : default; }

    /// <returns>True if the grid address is within the grid's dimensions</returns>
    public bool SetObjectAt(uint x, uint y, uint z, TGridObject gridObject)
    {
        if(!IsWithinBounds(x, y, z)) return false;
        _objects[x, y, z] = gridObject;
        return true;
    }

    public bool IsWithinBounds(uint x, uint y, uint z)
        { return !(x >= XSize || y >= YSize || z >= ZSize); }
    public bool IsWithinBounds(int x, int y, int z)
        { return !(x < 0 || y < 0 || z < 0 || x >= XSize || y >= YSize || z >= ZSize); }
}
