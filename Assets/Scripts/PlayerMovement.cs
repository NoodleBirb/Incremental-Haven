using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Dictionary<Vector2Int, GameObject> map;
    List<GameObject> movementPath;
    bool readyToMove = false;
    Vector2Int technicalPos;
    public float speed;


    void Start() {
        if (StoreTileMap.isMapInitialized) {
            ReadyMovement();
        } else {
            StoreTileMap.OnMapInitialized += PlayerMovementInitialized;
        }
        technicalPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {   
        
        if (readyToMove && Input.GetMouseButtonDown(0)) {
            
            // Create a ray based on the mouse click.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // cast ray
            if (Physics.Raycast (ray, out RaycastHit hit)) {
                
                // check if ray is far enough from the player
                if (Vector3.Distance(transform.position, hit.point) < 1) return;
                Vector2Int playerPos = new(technicalPos.x, technicalPos.y);
                
                GameObject endTile;
                
                // Account for the ray being converted to an integer having a truncation error.
                if (hit.point.x < 0 && hit.point.z < 0) {
                    endTile = map[new Vector2Int((int)(hit.point.x) - 1, (int)(hit.point.z) - 1)];
                } else if (hit.point.x < 0) {
                    endTile = map[new Vector2Int((int)(hit.point.x) - 1, (int)(hit.point.z))];
                } else if (hit.point.z < 0) {
                    endTile = map[new Vector2Int((int)(hit.point.x), (int)(hit.point.z) - 1)];
                } else {
                    endTile = map[new Vector2Int((int)(hit.point.x), (int)(hit.point.z))];
                }
                
                // Pathfind to the point found by the ray.
                movementPath = GetComponent<Pathfinding>().GetAStarPath(map[playerPos], endTile);
                if(Vector3.Distance(transform.position, new(technicalPos.x, 0, technicalPos.y)) > 0.00001f){
                    movementPath.Insert(0, map[new(technicalPos.x, technicalPos.y)]);
                }
                
                technicalPos = new((int)movementPath[0].transform.position.x, (int)movementPath[0].transform.position.z);
            }
        }
        // Checks if a movement path is currently being run through.
        if (movementPath != null && movementPath.Count > 0) {
            transform.position = Vector3.MoveTowards(transform.position, new(technicalPos.x, 0, technicalPos.y), speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, new(technicalPos.x, 0, technicalPos.y)) < 0.00001f){
                ReadyNextMovement();
            }
            
        }
    }

    void ReadyNextMovement() {
        movementPath.Remove(movementPath[0]);
        if (movementPath.Count > 0) {
            technicalPos = new((int)movementPath[0].transform.position.x, (int)movementPath[0].transform.position.z);
        }
    }

    void PlayerMovementInitialized() {
        ReadyMovement();
    }
    void ReadyMovement () {
        map = GameObject.Find("Game Map").GetComponent<StoreTileMap>().map;
        readyToMove = true;
    }

}
