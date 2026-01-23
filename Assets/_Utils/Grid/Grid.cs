
using System;

[Serializable] public class Grid<TGridObject>
{
    public uint XSize { get; private set; }
    public uint YSize { get; private set; }
    public uint ZSize { get; private set; }
    public int Left { get; private set; }
    public int Right { get; private set; }
    public int Top { get; private set; }
    public int Bottom { get; private set; }
    public int Front { get; private set; }
    public int Back { get; private set; }
    public TGridObject[,,] _objects = new TGridObject[1,1,1];
    public void Resize(int right, int top, int back, int left = 0, int bottom = 0, int front = 0)
    {
        if(left >= right || bottom >= top || front >= back)
        {
            LogUtils.LogWarning("Invalid Grid Dimensions Set");
            return;
        }

        uint oldXSize = XSize;
        uint oldYSize = YSize;
        uint oldZSize = ZSize;

        Right = right;      Top = top;          Back = back;
        Left = left;        Bottom = bottom;    Front = front;
        XSize = (uint)(Right - Left);
        YSize = (uint)(Top - Bottom);
        ZSize = (uint)(Back - Front);

        if (_objects == null)
        {
            _objects = new TGridObject[XSize, YSize, ZSize];
            return;
        }

        TGridObject[,,] newArray = new TGridObject[XSize, YSize, ZSize];

        uint minX = Math.Min(XSize, oldXSize);
        uint minY = Math.Min(YSize, oldYSize);
        uint minZ = Math.Min(ZSize, oldZSize);
        
        for (uint x = 0; x < minX; ++x)
        for (uint y = 0; y < minY; ++y)
        for (uint z = 0; z < minZ; ++z)
            newArray[x, y, z] = _objects[x, y, z];

        _objects = newArray;
    }

    /// <returns>True if there is an object at the indicated cell.</returns>
    public bool GetObjectAtCell(int x, int y, int z, out TGridObject obj)
    {  
        obj = IsWithinBounds(x, y, z) ? _objects[x - Left, y - Bottom, z - Front] : default; 
        return !Equals(obj, default);
    }

    /// <returns>True if the grid address is within the grid's dimensions</returns>
    public bool SetObjectAt(int x, int y, int z, TGridObject gridObject)
    {
        LogUtils.Log($"Set object at {x}, {y}, {z}");
        if(!IsWithinBounds(x, y, z)) return false;
        _objects[x - Left, y - Bottom, z - Front] = gridObject;
        return true;
    }
    public bool IsWithinBounds(int x, int y, int z)
        { return !(x < Left || y < Bottom || z < Front || x > Right || y > Top || z > Back); }
}
