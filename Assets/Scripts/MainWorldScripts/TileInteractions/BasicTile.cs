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
        AccessMap();
    }

    void AccessMap() {
        Dictionary<Vector2Int, GameObject> map = StoreTileMap.map;

        neighbors = new List<GameObject>();
        pos = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        if (map.ContainsKey(pos + new Vector2Int(1, 0))) {
            neighbors.Add(map[pos + new Vector2Int(1, 0)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(1, 1))) {
            neighbors.Add(map[pos + new Vector2Int(1, 1)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(0, 1))) {
            neighbors.Add(map[pos + new Vector2Int(0, 1)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(-1, 1))) {
            neighbors.Add(map[pos + new Vector2Int(-1, 1)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(-1, 0))) {
            neighbors.Add(map[pos + new Vector2Int(-1, 0)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(-1, -1))) {
            neighbors.Add(map[pos + new Vector2Int(-1, -1)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(0, -1))) {
            neighbors.Add(map[pos + new Vector2Int(0, -1)]);
        }
        if (map.ContainsKey(pos + new Vector2Int(1, -1))) {
            neighbors.Add(map[pos + new Vector2Int(1, -1)]);
        }
    }

}
