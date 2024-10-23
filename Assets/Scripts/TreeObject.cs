using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObject : MonoBehaviour, InteractableObject
{
    private readonly int personalGUIHeight = 50;
    private int treeTime;
    private bool treeCutTime;
    public bool isCut;
    
    void Start() {
        isCut = false;
        treeTime = 0;
        treeCutTime = false;
    }

    void Update() {
        if (treeCutTime) {
            GameObject Player = GameObject.Find("Player");
            StartCoroutine(CutTree(Player));
        }
    }

    public void CreateOptions(int previousHeight, Vector2 clickPos, int totalGUIWidth) {
        if (GUI.Button(new Rect(clickPos.x, Screen.height - clickPos.y + previousHeight, totalGUIWidth, personalGUIHeight), "Chop Tree")) {
            Vector2Int pos = GetComponentInParent<BasicTile>().pos;
            GameObject Player = GameObject.Find("Player");
            if (Vector3.Distance(Player.transform.position, new(pos.x, 0, pos.y)) > 1){
                Player.GetComponent<PlayerMovement>().GuiMovement(pos);
            }
            if (!isCut) {
                treeCutTime = true;
            }
            Player.GetComponent<PlayerMovement>().openGUI = false;
        }
    }

    public int GetGUIHeight() {
        return personalGUIHeight;
    }


    IEnumerator CutTree(GameObject player) {
        if (player.GetComponent<PlayerMovement>().movementPath.Count == 0) {
            treeTime += 1;
        }
        if (treeTime == 300) {
            player.GetComponent<Skills>().skillList["Woodcutting"].IncreaseEXP(20);
            StopCoroutine(CutTree(player));
            treeCutTime = false;
            treeTime = 0;
            isCut = true;
        }
        yield return new WaitForSeconds(.1f);
    }
}