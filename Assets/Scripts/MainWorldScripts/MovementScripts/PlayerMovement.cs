using System;
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
    static Vector2Int technicalPos; // arbitrary numbers that are most definitely impossible to reach
    public float speed;
    public static bool openGUI;
    GameObject guiTile = null;
    Vector2 clickPos;
    Vector2 guiPos;
    int latestClick;
    public static event Action ResetActions;


    void Start() {

        openGUI = false;
        if (StoreTileMap.isMapInitialized) {
            ReadyMovement();
        } else {
            StoreTileMap.OnMapInitialized += PlayerMovementInitialized;
        }
        if (technicalPos == new Vector2Int(0, 0)) {
            technicalPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        } else {
            StartCoroutine(WaitForTransform(transform, () => {
                transform.position = new(technicalPos.x, 0, technicalPos.y);
            }));
            
        }
        ResetActions?.Invoke();
    }
    // Update is called once per frame
    void Update()
    {   
        
        if (!Inventory.showInventory) {
            // Left click movement
            if (readyToMove && Input.GetMouseButtonDown(0)) {
                latestClick = 0;
                clickPos = new(Input.mousePosition.x, Input.mousePosition.y);
                SendRay(0);
            }
            // Right click gui creation
            if (readyToMove && Input.GetMouseButtonUp(1)) {
                latestClick = 1;
                clickPos = new(Input.mousePosition.x, Input.mousePosition.y); 
                SendRay(1);
            }
            // Checks if a movement path is currently being run through.
            if (movementPath != null && movementPath.Count > 0) {
                GameObject.Find("player model").transform.LookAt(new Vector3(technicalPos.x + 0.5f, 0, technicalPos.y + 0.5f));
                transform.position = Vector3.MoveTowards(transform.position, new(technicalPos.x, 0, technicalPos.y), speed * Time.deltaTime);
                if(Vector3.Distance(transform.position, new(technicalPos.x, 0, technicalPos.y)) < 0.00001f){
                    ReadyNextMovement();
                }
                
            }
        }
    }
    IEnumerator WaitForTransform(Transform transform, System.Action loadPos) {
        yield return new WaitUntil(() => transform != null);
        loadPos?.Invoke();
    }

    // Sends a ray based on the user's input and runs either movement or GUI creation
    void SendRay(int mouseInput) {
        // Create a ray based on the mouse click.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // cast ray
        if (Physics.Raycast (ray, out RaycastHit hit)) {
            Vector2 rectMousePos = new(clickPos.x, Screen.height - clickPos.y);
            if (InsideGUIBox()) {
                Debug.Log("functioning correctly");
                return;
            }
            if (mouseInput == 0) {
                openGUI = false;
                BeginMovement(GetActualRayTile(hit));
                /* test that each tile has a value set correctly for its neighbors
                foreach (GameObject tile in map.Values) {
                    Debug.Log("New Tile: ");
                    Debug.Log("x pos: " + tile.transform.position.x + " z pos: " + tile.transform.position.z);
                    Debug.Log("Neighbors: ");
                    foreach (GameObject neighbor in tile.GetComponent<BasicTile>().neighbors) {
                        Debug.Log("x pos: " + neighbor.transform.position.x + " z pos: " + neighbor.transform.position.z);
                    }
                } */
            }
            else if (mouseInput == 1) {
                ReadyGUI(hit);
            }
        }
    }

    // Move to the raycasted tile.
    public void BeginMovement(GameObject endTile) {
       
        ResetActions?.Invoke();
        if (endTile.GetComponent<TileSettings>().heldObject != null) {
            endTile.GetComponent<TileSettings>().heldObject.GetComponent<InteractableObject>().InteractWith();
        }
        
        Vector2Int playerPos = new(technicalPos.x, technicalPos.y);
                
        // Pathfind to the point found by the ray.
        movementPath = Pathfinding.GetAStarPath(map[playerPos], endTile);
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
        map = StoreTileMap.map;
        readyToMove = true;
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

    // openGUI = false may cause problems if I want to use this checking code for things other than clicks
    bool InsideGUIBox(Vector2 rectMousePos) {
        
        Skills skills = GetComponent<Skills>();
        Inventory inventory = GetComponent<Inventory>();
        if (skills.skillListRect.Contains(rectMousePos)) {
            openGUI = false;
            return true;
        }
        if (openGUI) {
            TileSettings guiTileSettings = guiTile.GetComponent<TileSettings>();
            if (guiTileSettings.fullRectSize.Contains(rectMousePos)) {
                return true;
            }
        }
        if (skills.showSkillList && skills.windowRect.Contains(rectMousePos)) {
            openGUI = false;
            return true;
        }
        if (inventory.openInventoryRect.Contains(rectMousePos)) {
            openGUI = false;
            return true;
        }
        //if (inventory)

        return false;
    }

    bool InsideGUIBox() {
        RectTransform inventoryRect = GameObject.Find("Inventory Button").GetComponent<RectTransform>();
        Vector2 localMousePosition = inventoryRect.InverseTransformPoint(Input.mousePosition);
        if (inventoryRect.rect.Contains(localMousePosition)) {
            return true;
        }
        RectTransform skillsRect = GameObject.Find("Skills Button").GetComponent<RectTransform>();
        localMousePosition = skillsRect.InverseTransformPoint(Input.mousePosition);
        if (skillsRect.rect.Contains(localMousePosition)) {
            return true;
        }
        return false;
    }

    public Vector2Int GetTechnicalPos() {
        return technicalPos;
    }
}
