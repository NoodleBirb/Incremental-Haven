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
    public static event Action ResetActions;

    void Awake() {
        ResetActions = null;
    }

    void Start() {

        openGUI = false;
        ReadyMovement();
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
                SendRay(0);
            }
            // Right click gui creation
            if (readyToMove && Input.GetMouseButtonUp(1)) {
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
            if (InsideGUIBox()) {
                return;
            }
            if (mouseInput == 0) {
                openGUI = false;
                GameObject tile = hit.collider.transform.parent.gameObject;
                while (tile.GetComponent<TileSettings>() == null || (tile.GetComponent<TileSettings>() != null && !tile.GetComponent<TileSettings>().baseTile)) {
                    tile = tile.transform.parent.gameObject;
                }
                BeginMovement(tile);
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
    public void BeginMovement(GameObject endTile, bool gui = false, bool campfire = false) {
       
        ResetActions?.Invoke();
        
        Vector2Int playerPos = new(technicalPos.x, technicalPos.y);
                
        // Pathfind to the point found by the ray.
        movementPath = Pathfinding.GetAStarPath(map[playerPos], endTile);
        if(Vector3.Distance(transform.position, new(technicalPos.x, 0, technicalPos.y)) > 0.00001f){
            movementPath.Insert(0, map[new(technicalPos.x, technicalPos.y)]);
        }
        if (movementPath.Count > 0){       
            technicalPos = new((int)movementPath[0].transform.position.x, (int)movementPath[0].transform.position.z);
        }
        if (endTile.GetComponent<TileSettings>().heldObject != null && !gui) {
            endTile.GetComponent<TileSettings>().heldObject.GetComponent<InteractableObject>().InteractWith();
        }
        if (campfire && movementPath.Count > 0) {
            movementPath.RemoveAt(movementPath.Count - 1);
        }
    }

    // Readies the next piece of the path.
    void ReadyNextMovement() {
        movementPath.Remove(movementPath[0]);
        if (movementPath.Count > 0) {
            if (movementPath[0].GetComponent<TileSettings>().walkable) {
                technicalPos = new((int)movementPath[0].transform.position.x, (int)movementPath[0].transform.position.z);
            } else {
                movementPath = new();
            }
        }
    }

    // Readies the player for movement as the map has been initialized.
    void ReadyMovement () {
        map = StoreTileMap.map;
        readyToMove = true;
    }

    // Readies the screen to have a gui, giving it the proper information.
    void ReadyGUI (RaycastHit hit) {
        guiTile = hit.collider.transform.parent.gameObject;
        while (guiTile.GetComponent<TileSettings>() == null || (guiTile.GetComponent<TileSettings>() != null && !guiTile.GetComponent<TileSettings>().baseTile)) {
            guiTile = guiTile.transform.parent.gameObject;
        }
        guiTile.GetComponent<TileSettings>().GuiOptions();
        openGUI = true;
    }

    bool InsideGUIBox() {
        Vector2 localMousePosition;
        foreach (Transform popup in GameObject.Find("Interaction Container").transform) {
            localMousePosition = popup.gameObject.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
            if (popup.gameObject.GetComponent<RectTransform>().rect.Contains(localMousePosition)) {
                return true;
            }
        }
        InteractableObject.ResetGUI();
        RectTransform inventoryRect = GameObject.Find("Inventory Button").GetComponent<RectTransform>();
        localMousePosition = inventoryRect.InverseTransformPoint(Input.mousePosition);
        if (inventoryRect.rect.Contains(localMousePosition)) {
            return true;
        }
        RectTransform skillsRect = GameObject.Find("Skills Button").GetComponent<RectTransform>();
        localMousePosition = skillsRect.InverseTransformPoint(Input.mousePosition);
        if (skillsRect.rect.Contains(localMousePosition)) {
            return true;
        }
        RectTransform skillListRect = GameObject.Find("Skill List Package").GetComponent<RectTransform>();
        localMousePosition = skillListRect.InverseTransformPoint(Input.mousePosition);
        if (GameObject.Find("Skill List Canvas").GetComponent<Canvas>().enabled && skillListRect.rect.Contains(localMousePosition)) {
            return true;
        }
        RectTransform furnaceWindowRect = GameObject.Find("Smelting Window").GetComponent<RectTransform>();
        localMousePosition = furnaceWindowRect.InverseTransformPoint(Input.mousePosition);
        if (GameObject.Find("Furnace Canvas").GetComponent<Canvas>().enabled && furnaceWindowRect.rect.Contains(localMousePosition)) {
            return true;
        }
        RectTransform campfireWindowRect = GameObject.Find("Smelting Window").GetComponent<RectTransform>();
        localMousePosition = campfireWindowRect.InverseTransformPoint(Input.mousePosition);
        if (GameObject.Find("Campfire Canvas").GetComponent<Canvas>().enabled && furnaceWindowRect.rect.Contains(localMousePosition)) {
            return true;
        }
        foreach (Transform popup in GameObject.Find("Popup Container").transform) {
            localMousePosition = popup.gameObject.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
             if (popup.gameObject.GetComponent<RectTransform>().rect.Contains(localMousePosition)) {
                return true;
             }
        }
        return false;
    }

    public Vector2Int GetTechnicalPos() {
        return technicalPos;
    }
}
