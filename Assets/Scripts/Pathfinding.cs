using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    // GameObject must be a Tile.
    Queue<Vector2> getAStarPath(GameObject startTile, GameObject endTile) {
        
        List<GameObject> openList = new();
        List<GameObject> closedList = new();
        openList.Add(startTile);

        while (openList.Count > 0) {

            GameObject currentTile = openList[0];
            for (int i = 1; i < openList.Count; i++) {

            }
        }
        return null; //placeholder
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
