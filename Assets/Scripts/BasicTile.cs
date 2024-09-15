using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class BasicTile : MonoBehaviour
{
    public List<GameObject> neighbors;
    Vector2Int pos;
    // Start is called before the first frame update
    void Start()
    {
        if (StoreTileMap.isMapInitialized) AccessMap();
        else StoreTileMap.OnMapInitialized += HandleMapInitialized;

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
        }
        if (map.ContainsKey(pos + new Vector2Int(1, 1)) && map[pos + new Vector2Int(1, 1)].GetComponentInChildren<objectSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(1, 1)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(0, 1)) && map[pos + new Vector2Int(0, 1)].GetComponentInChildren<objectSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(0, 1)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(-1, 1)) && map[pos + new Vector2Int(-1, 1)].GetComponentInChildren<objectSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(-1, 1)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(-1, 0)) && map[pos + new Vector2Int(-1, 0)].GetComponentInChildren<objectSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(-1, 0)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(-1, -1)) && map[pos + new Vector2Int(-1, -1)].GetComponentInChildren<objectSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(-1, -1)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(0, -1)) && map[pos + new Vector2Int(0, -1)].GetComponentInChildren<objectSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(0, -1)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(1, -1)) && map[pos + new Vector2Int(1, -1)].GetComponentInChildren<objectSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(1, -1)]);
        }
    }

}
