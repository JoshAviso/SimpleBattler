
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private GridGameObject _placeObj;

    public static bool PlaceObject()
    {
        if(!Instance || !Instance._placeObj) return false;

        Vector3Int placepos = ObjectPlacementGrid.Instance.PlacementPos;
        bool res = ObjectPlacementGrid.Instance.TrySetObject(Instance._placeObj, placepos.x, placepos.y, placepos.z);
        
        if(!res) return false;

        GameObject obj = Instantiate(Instance._placeObj.Prefab);
        obj.transform.position = ObjectPlacementGrid.Instance.CellToWorld(placepos.x, placepos.y, placepos.z);    
        obj.SetActive(true);    

        return true;
    }

    public void TryPlace()
    {
        _ = PlaceObject();
    }

    // SINGLETON
    public static ObjectPlacer Instance { get; private set; }
    protected virtual void Awake(){
        if (Instance == null) {
            Instance = this;
            // DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }
    }
}
