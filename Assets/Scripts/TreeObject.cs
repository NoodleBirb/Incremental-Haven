using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObject : MonoBehaviour, InteractableObject
{
    private readonly int personalGUIHeight = 50;
    
    void Start() {
        
    }

    public void CreateOptions(int previousHeight, Vector2 clickPos, int totalGUIWidth) {
        if (GUI.Button(new Rect(clickPos.x, Screen.height - clickPos.y + previousHeight, totalGUIWidth, personalGUIHeight), "Chop Tree")) {
            Vector2Int pos = GetComponentInParent<BasicTile>().pos;
            GameObject Player = GameObject.Find("Player");
            Player.GetComponent<PlayerMovement>().GuiMovement(pos);
            Player.GetComponent<PlayerMovement>().openGUI = false;
            Player.GetComponent<Skills>().skillList["Woodcutting"] += 20;
        }
    }

    public int GetGUIHeight() {
        return personalGUIHeight;
    }

}