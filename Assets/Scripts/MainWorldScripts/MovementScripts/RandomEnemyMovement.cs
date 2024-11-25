using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomEnemyMovement : MonoBehaviour {
    private Vector2Int technicalPos;
    List<GameObject> movementPath;
    Dictionary<Vector2Int, GameObject> map;
    bool readyToMove;
    readonly int speed = 5;
    

    void Start() {
        technicalPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        readyToMove = false;
        if (StoreTileMap.isMapInitialized) {
            ReadyMovement();
        } else {
            StoreTileMap.OnMapInitialized += ReadyMovement;
        }
        movementPath = new();
    }

    void ReadyMovement () {
        map = StoreTileMap.map;
        readyToMove = true;
    }

    void Update() {
        if (!Inventory.showInventory) { 
            Vector2Int playerTechnicalPos = GameObject.Find("Player").GetComponent<PlayerMovement>().GetTechnicalPos();
            if (technicalPos.x == playerTechnicalPos.x && technicalPos.y == playerTechnicalPos.y) {
                GetComponent<EnemyStatistics>().BumpedIntoPlayer();
            }
            if (readyToMove && movementPath.Count == 0 && (int)(Random.value * 100) == 1) {
                BeginMovement(Enumerable.ToList<GameObject>(map.Values)[(int)(Random.value * map.Count)]);
            }
            // Checks if a movement path is currently being run through.
            if (movementPath != null && movementPath.Count > 0) {
                transform.position = Vector3.MoveTowards(transform.position, new(technicalPos.x, 0, technicalPos.y), speed * Time.deltaTime);
                if(Vector3.Distance(transform.position, new(technicalPos.x, 0, technicalPos.y)) < 0.00001f){
                    ReadyNextMovement();
                }
                    
            }
        }
    }

    public void BeginMovement(GameObject endTile) {
        
        Vector2Int enemyPos = new(technicalPos.x, technicalPos.y);
                
        // Pathfind to the point found by the ray.
        movementPath = Pathfinding.GetAStarPath(map[enemyPos], endTile);
        if(Vector3.Distance(transform.position, new(technicalPos.x, 0, technicalPos.y)) > 0.00001f){
            movementPath.Insert(0, map[new(technicalPos.x, technicalPos.y)]);
        }
        if (movementPath.Count > 0){       
            technicalPos = new((int)movementPath[0].transform.position.x, (int)movementPath[0].transform.position.z);
        }
    }
    // Readies the next piece of the path.
    void ReadyNextMovement() {
        movementPath.Remove(movementPath[0]);
        if (movementPath.Count > 0) {
            technicalPos = new((int)movementPath[0].transform.position.x, (int)movementPath[0].transform.position.z);
        }
    }

}