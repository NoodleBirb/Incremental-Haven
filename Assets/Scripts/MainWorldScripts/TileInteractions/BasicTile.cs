using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class BasicTile : MonoBehaviour
{
    public List<GameObject> neighbors;
    public Vector2Int pos;
    // Start is called before the first frame update
    void Start()
    {
        if (StoreTileMap.isMapInitialized) AccessMap();
        else StoreTileMap.OnMapInitialized += HandleMapInitialized;

    }
    void OnDestroy() {
        StoreTileMap.OnMapInitialized -= HandleMapInitialized;
    }

    void HandleMapInitialized()
    {
        AccessMap();
    }


    void AccessMap() {
        Dictionary<Vector2Int, GameObject> map = StoreTileMap.map;

        neighbors = new List<GameObject>();
        pos = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        if (map.ContainsKey(pos + new Vector2Int(1, 0)) && map[pos + new Vector2Int(1, 0)].GetComponentInChildren<TileSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(1, 0)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(1, 1)) && map[pos + new Vector2Int(1, 1)].GetComponentInChildren<TileSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(1, 1)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(0, 1)) && map[pos + new Vector2Int(0, 1)].GetComponentInChildren<TileSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(0, 1)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(-1, 1)) && map[pos + new Vector2Int(-1, 1)].GetComponentInChildren<TileSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(-1, 1)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(-1, 0)) && map[pos + new Vector2Int(-1, 0)].GetComponentInChildren<TileSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(-1, 0)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(-1, -1)) && map[pos + new Vector2Int(-1, -1)].GetComponentInChildren<TileSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(-1, -1)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(0, -1)) && map[pos + new Vector2Int(0, -1)].GetComponentInChildren<TileSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(0, -1)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(1, -1)) && map[pos + new Vector2Int(1, -1)].GetComponentInChildren<TileSettings>().walkable) {
            neighbors.Add(map[pos + new Vector2Int(1, -1)]);
        }
    }

}
