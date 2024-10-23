using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    Dictionary<Vector2Int, GameObject> map;
    public List<GameObject> movementPath;
    bool readyToMove;
    Vector2Int technicalPos;
    public float speed;
    public bool openGUI;
    GameObject guiTile = null;
    Vector2 clickPos;
    Vector2 guiPos;
    int latestClick;


    void Start() {
        openGUI = false;
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
            latestClick = 0;
            clickPos = new(Input.mousePosition.x, Input.mousePosition.y);
          
            SendRay(0);
        }
        if (readyToMove && Input.GetMouseButtonUp(1)) {
            latestClick = 1;
            clickPos = new(Input.mousePosition.x, Input.mousePosition.y); 
            SendRay(1);
        }
        // Checks if a movement path is currently being run through.
        if (movementPath != null && movementPath.Count > 0) {
            transform.position = Vector3.MoveTowards(transform.position, new(technicalPos.x, 0, technicalPos.y), speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, new(technicalPos.x, 0, technicalPos.y)) < 0.00001f){
                ReadyNextMovement();
            }
            
        }
    }

    // Sends a ray based on the user's input and runs either movement or GUI creation
    void SendRay(int mouseInput) {
        // Create a ray based on the mouse click.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // cast ray
            if (Physics.Raycast (ray, out RaycastHit hit)) {
                Vector2 rectMousePos = new(clickPos.x, Screen.height - clickPos.y);
                if (InsideGUIBox(rectMousePos)) {
                    return;
                }
                if (mouseInput == 0) {
                    openGUI = false;
                    //if (Vector3.Distance(transform.position, hit.point) < 1) return;
                    BeginMovement(GetActualRayTile(hit));
                }
                else if (mouseInput == 1) {
                    ReadyGUI(hit);
                }
            }
    }

    // Move to the raycasted tile.
    void BeginMovement(GameObject endTile) {
        
        Vector2Int playerPos = new(technicalPos.x, technicalPos.y);
                
        // Pathfind to the point found by the ray.
        movementPath = GetComponent<Pathfinding>().GetAStarPath(map[playerPos], endTile);
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

    // Event call for map initialization
    void PlayerMovementInitialized() {
        ReadyMovement();
    }

    // Readies the player for movement as the map has been initialized.
    void ReadyMovement () {
        map = GameObject.Find("Game Map").GetComponent<StoreTileMap>().map;
        readyToMove = true;
    }

    // Movement code particular to when the user clicks on a GUI for movement.
    public void GuiMovement (Vector2Int tilePos) {
        Vector2Int playerPos = new(technicalPos.x, technicalPos.y);

        // Pathfind to the point found by the ray.
        movementPath = GetComponent<Pathfinding>().GetAStarPath(map[playerPos], map[tilePos]);
        if(Vector3.Distance(transform.position, new(technicalPos.x, 0, technicalPos.y)) > 0.00001f){
            movementPath.Insert(0, map[new(technicalPos.x, technicalPos.y)]);
        }
                
        technicalPos = new((int)movementPath[0].transform.position.x, (int)movementPath[0].transform.position.z);
    }

    // Readies the screen to have a gui, giving it the proper information.
    void ReadyGUI (RaycastHit hit) {
        guiTile = GetActualRayTile(hit);
        guiPos = clickPos;
        openGUI = true;
    }

    // Fix rounding errors when converting the hit to integer.
    GameObject GetActualRayTile(RaycastHit hit) {
        if (hit.point.x < 0 && hit.point.z < 0) {
            return map[new Vector2Int((int)hit.point.x - 1, (int)hit.point.z - 1)];
        } else if (hit.point.x < 0) {
            return map[new Vector2Int((int)hit.point.x - 1, (int)hit.point.z)];
        } else if (hit.point.z < 0) {
            return map[new Vector2Int((int)hit.point.x, (int)hit.point.z - 1)];
        } else {
            return map[new Vector2Int((int)hit.point.x, (int)hit.point.z)];
        }
    }

    // Create the clickable guis based on the tile
    void OnGUI () {
        if (openGUI) {
            guiTile.GetComponent<TileSettings>().GuiOptions(guiPos, latestClick);
        }
    }

    bool InsideGUIBox(Vector2 rectMousePos) {
        Skills skills = GetComponent<Skills>();
        if (skills.skillListRect.Contains(rectMousePos)) {
            return true;
        }
        if (openGUI) {
            TileSettings guiTileSettings = guiTile.GetComponent<TileSettings>();
            if (guiTileSettings.fullRectSize.Contains(rectMousePos)) {
                return true;
            }
        }
        if (skills.showSkillList && skills.windowRect.Contains(rectMousePos)) {
            Debug.Log("this should be working");
            return true;
        }

        return false;
    }
}
