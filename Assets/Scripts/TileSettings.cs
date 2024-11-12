using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSettings : MonoBehaviour
{

    // For usage in pathfinding.
    public bool walkable = true;
    // For logic with GUIBoxes
    public GameObject heldObject = null;
    // Value from adding the heights of the tile's objects gui boxes together.
    public int guiHeight = 0;
    // for creating boxes with the correct heights
    readonly int personalGUIHeight = 50;
    public readonly int totalGUIWidth = 250;
    public Vector2 guiPos = new(-1, -1);
    public Rect fullRectSize;

    void Start() {
        guiHeight = personalGUIHeight;
        if (heldObject != null) {
            guiHeight += heldObject.GetComponent<InteractableObject>().GetGUIHeight();
        }
        fullRectSize = new(1000000, 1000000, totalGUIWidth, guiHeight);
    }
    
    public void GuiOptions (Vector2 clickPos, int latestClick) {
        guiPos = clickPos;
        fullRectSize = new(clickPos.x, Screen.height - clickPos.y, totalGUIWidth, guiHeight);

        if (GUI.Button(new Rect(clickPos.x, Screen.height - clickPos.y, totalGUIWidth, personalGUIHeight), "Walk Towards") && latestClick == 0) {
            Vector2Int pos = GetComponent<BasicTile>().pos;
            GameObject player = GameObject.Find("Player");
            player.GetComponent<PlayerMovement>().BeginMovement(gameObject);

            
            player.GetComponent<PlayerMovement>().openGUI = false;
        }
        if (heldObject != null) {
            InteractableObject interactable = heldObject.GetComponent<InteractableObject>();
            interactable?.CreateOptions(personalGUIHeight, clickPos, totalGUIWidth);
        }
    }
}