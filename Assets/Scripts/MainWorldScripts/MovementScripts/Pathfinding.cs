using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    // Helper Class
    private class Node {
        private Node parentNode;
        private readonly GameObject tile;

        public Node (GameObject tile, Node parentNode) {
            this.parentNode = parentNode;
            this.tile = tile;
        }

        public Node GetParentNode() {
            return parentNode;
        }
        public GameObject GetTile() {
            return tile;
        }
        public void ChangeParentNode(Node newNode) {
            parentNode = newNode;
        }
    }

    // GameObject must be a Tile.
    public static List<GameObject> GetAStarPath(GameObject startTile, GameObject goalTile) {

        

        List<GameObject> openList = new();
        List<GameObject> closedList = new();

        // Tracks nodes and allows for easy access to them
        Dictionary<GameObject, Node> nodeDict = new();

        openList.Add(startTile);
        nodeDict.Add(startTile, new Node(startTile, null));

        // Allows us to get a path even if there is no way to reach the destination
        Node lowestDistNode = nodeDict[startTile];

        // While there are still tiles to check
        while (openList.Count > 0) {

            GameObject currentTile = openList[0];
            int currentFCost = CalcFCost(currentTile, startTile, goalTile);

            // Set the currentTile based on whichever one in openList has the lowest FCost
            for (int i = 1; i < openList.Count; i++) {
                if (CalcFCost(openList[i], startTile, goalTile) < currentFCost) {
                    currentTile = openList[i];
                    currentFCost = CalcFCost(openList[i], startTile, goalTile);
                }
            }
            closedList.Add(currentTile);
            openList.Remove(currentTile);

            // If you've reached the goal, no reason to continue. Returns the path.
            if (currentTile == goalTile) {
                return GetActualPath(nodeDict[goalTile]);
            }
            // Check every neighbor of the currentTile to see if they can find a good path.
            foreach (GameObject neighbor in currentTile.GetComponent<BasicTile>().neighbors) {
                if (closedList.Contains(neighbor) || !neighbor.GetComponent<TileSettings>().walkable) continue;
                
                // Checks if we've used this tile before
                if (!openList.Contains(neighbor)) {
                    openList.Add(neighbor);
                    nodeDict.Add(neighbor, new Node(neighbor, nodeDict[currentTile]));
                    if (CalcGCost(neighbor, goalTile) < CalcGCost(lowestDistNode.GetTile(), goalTile)) {
                        lowestDistNode = nodeDict[neighbor];
                    }
                }
                // If we have, check if this is a more efficient path to said tile
                else if (CalcGCost(currentTile, startTile) + GetDistance(currentTile, neighbor) < CalcGCost(neighbor, startTile)) {
                    nodeDict[neighbor].ChangeParentNode(nodeDict[currentTile]);
                }
            }
        }
        return GetActualPath(lowestDistNode); // closest path to the destination if you can't reach the destination.
    }
    
    static int  CalcFCost (GameObject tile, GameObject startTile, GameObject goalTile) {
        return GetDistance(tile, startTile) + GetDistance(tile, goalTile);
    }

    static int CalcGCost (GameObject tile, GameObject startTile) {
        return GetDistance(tile, startTile);
    }

    static int GetDistance (GameObject tile1, GameObject tile2) {
        int dstX = (int)Mathf.Abs(tile1.transform.position.x - tile2.transform.position.x);
        int dstZ = (int)Mathf.Abs(tile1.transform.position.z - tile2.transform.position.z);

        if (dstX > dstZ) {
            return 140 * dstZ + 100 * (dstX - dstZ);
        }
        return 140 * dstX + 100 * (dstZ - dstX);
    }

    static List<GameObject> GetActualPath(Node finalNode) {
        List<GameObject> retList = new();
        while (finalNode.GetParentNode() != null) {
            retList.Add(finalNode.GetTile());
            finalNode = finalNode.GetParentNode();
        }
        retList.Reverse();
        return retList;
    }
}
