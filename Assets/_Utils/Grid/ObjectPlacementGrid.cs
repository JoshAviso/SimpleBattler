
using UnityEngine;

public class ObjectPlacementGrid : GridComponent<GameObject>
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

    public bool TrySetObject(GameObject obj, uint x, uint y, uint z)
    {
        if(!IsWithinBounds(x, y, z)) return false;
        if(GetObjectAtCell(x, y, z) != null) return false;

        SetObjectAt(x, y, z, obj);
        return true;
    }

    public bool TrySetObject(GameObject obj, int x, int y, int z)
        { return TrySetObject(obj, (uint)x, (uint)y, (uint)z); }

    protected override string GetCellLabel(uint x, uint y, uint z)
    {
        if(playerCell.x == (int)x && playerCell.y == (int)y && playerCell.z == (int)z)
            return "Player Cell";
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

        if(WorldToCell(playerPos, out var x, out var y, out var z))
            playerCell = new((int)x, (int)y, (int)z);
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