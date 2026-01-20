
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private GameObject placeObj;

    public static bool PlaceObject()
    {
        if(!Instance || !Instance.placeObj) return false;


        GameObject obj = Instantiate(Instance.placeObj);
        obj.SetActive(false);
        Vector3Int placepos = ObjectPlacementGrid.Instance.PlacementPos;

        bool res = ObjectPlacementGrid.Instance.TrySetObject(obj, placepos.x, placepos.y, placepos.z);
        
        if(!res)
        {
            LogUtils.Log("Failed place object!");
            Destroy(obj);
            return false;
        }

        LogUtils.Log("Placed object!");
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
