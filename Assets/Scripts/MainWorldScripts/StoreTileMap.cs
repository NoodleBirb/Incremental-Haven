using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StoreTileMap : MonoBehaviour
{
    public static Dictionary<Vector2Int, GameObject> map;
    public static event Action OnMapInitialized;
    public static bool isMapInitialized = false;
    
    void Start()
    {
        
        // I'VE COOKED SO HARD USING A DICTIONARY!!! I HOPE IT'S FAST
        map = new Dictionary<Vector2Int, GameObject>();
        int count = 0;
        foreach (Transform child in transform) {
            map.Add(new Vector2Int((int)child.position.x, (int)child.position.z), child.gameObject);
            count++;
        }

        isMapInitialized = true;
        OnMapInitialized?.Invoke();
    }
    void OnDestroy() {
        isMapInitialized = false;
        map = null;
    }

}
