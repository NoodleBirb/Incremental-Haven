using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StoreTileMap : MonoBehaviour
{
    public Dictionary<Vector2Int, GameObject> map;
    public static event Action OnMapInitialized;
    public static bool isMapInitialized = false;
    
    void Start()
    {
        // I'VE COOKED SO HARD USING A DICTIONARY!!! I HOPE IT'S FAST
        map = new Dictionary<Vector2Int, GameObject>();

        foreach (Transform child in transform) {
            map.Add(new Vector2Int((int)child.position.x, (int)child.position.z), child.gameObject);
        }

        isMapInitialized = true;
        OnMapInitialized?.Invoke();
    }
}
