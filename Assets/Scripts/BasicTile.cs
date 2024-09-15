using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicTile : MonoBehaviour
{
    public List<GameObject> neighbors;
    Vector2Int pos;
    // Start is called before the first frame update
    void Start()
    {
        StoreTileMap.OnMapInitialized += HandleMapInitialized;

        pos = new Vector2Int((int)transform.position.x, (int)transform.position.z);

    }

    void HandleMapInitialized()
    {
        AccessMap();
    }

    void AccessMap() {
        Dictionary<Vector2Int, GameObject> map = GetComponentInParent<StoreTileMap>().map;

        neighbors = new List<GameObject>();

        
        if (map.ContainsKey(pos + new Vector2Int(1, 0)) && map[pos + new Vector2Int(1, 0)].GetComponentInChildren<objectSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(1, 0)]);
            Debug.Log("something is happening");
        }
        if (map.ContainsKey(pos + new Vector2Int(1, 1)) && map[pos + new Vector2Int(1, 1)].GetComponentInChildren<objectSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(1, 1)]);
            Debug.Log("something is happening");
        }
        if (map.ContainsKey(pos + new Vector2Int(0, 1)) && map[pos + new Vector2Int(0, 1)].GetComponentInChildren<objectSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(0, 1)]);
            Debug.Log("something is happening");
        }
        if (map.ContainsKey(pos + new Vector2Int(-1, 1)) && map[pos + new Vector2Int(-1, 1)].GetComponentInChildren<objectSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(-1, 1)]);
            Debug.Log("something is happening");
        }
        if (map.ContainsKey(pos + new Vector2Int(-1, 0)) && map[pos + new Vector2Int(-1, 0)].GetComponentInChildren<objectSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(-1, 0)]);
            Debug.Log("something is happening");
        }
        if (map.ContainsKey(pos + new Vector2Int(-1, -1)) && map[pos + new Vector2Int(-1, -1)].GetComponentInChildren<objectSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(-1, -1)]);
            Debug.Log("something is happening");
        }
        if (map.ContainsKey(pos + new Vector2Int(0, -1)) && map[pos + new Vector2Int(0, -1)].GetComponentInChildren<objectSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(0, -1)]);
            Debug.Log("something is happening");
        }
        if (map.ContainsKey(pos + new Vector2Int(1, -1)) && map[pos + new Vector2Int(1, -1)].GetComponentInChildren<objectSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(1, -1)]);
            Debug.Log("something is happening");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
