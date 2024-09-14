using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StoreMapInArray : MonoBehaviour
{
    Dictionary<Vector2Int, GameObject> map;
    
    void Start()
    {
        // I'VE COOKED SO HARD USING A DICTIONARY!!! I HOPE IT'S FAST
        map = new Dictionary<Vector2Int, GameObject>();

        foreach (Transform child in transform) {
            map.Add(new Vector2Int((int)child.position.x, (int)child.position.z), child.gameObject);
            Debug.Log(child.position.x);
        }
    }
}
