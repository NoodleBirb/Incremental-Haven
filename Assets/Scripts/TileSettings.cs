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

    void Start() {
        if (heldObject != null) {
            guiHeight = personalGUIHeight + heldObject.GetComponent<InteractableObject>().GetGUIHeight();
        }
    }
    
    public void GuiOptions (Vector2 clickPos, int latestClick) {
        guiPos = clickPos;

        if (walkable  && GUI.Button(new Rect(clickPos.x, Screen.height - clickPos.y, totalGUIWidth, personalGUIHeight), "Walk Towards") && latestClick == 0) {
            Vector2Int pos = GetComponent<BasicTile>().pos;
            GameObject.Find("Player").GetComponent<PlayerMovement>().GuiMovement(pos);
            GameObject.Find("Player").GetComponent<PlayerMovement>().openGUI = false;
        }
        if (heldObject != null) {
            InteractableObject interactable = heldObject.GetComponent<InteractableObject>();
            interactable?.CreateOptions(personalGUIHeight);
        }
    }
}