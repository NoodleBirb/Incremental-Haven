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
    GameObject Player;
    
    void Start() {
        isCut = false;
        treeTime = 0;
        treeCutTime = false;
        PlayerMovement.ResetActions += StopCuttingTree;
        Player = GameObject.Find("Player");
    }

    void Update() {
        if (treeCutTime && Player.GetComponent<PlayerMovement>().movementPath.Count == 0) {
            StartCoroutine(CutTree());
        }
    }

    public void CreateOptions(int previousHeight, Vector2 clickPos, int totalGUIWidth) {
        if (GUI.Button(new Rect(clickPos.x, Screen.height - clickPos.y + previousHeight, totalGUIWidth, personalGUIHeight), "Chop Tree")) {
            Vector2Int pos = GetComponentInParent<BasicTile>().pos;
            if (Vector3.Distance(Player.transform.position, new(pos.x, 0, pos.y)) > 1){
                Player.GetComponent<PlayerMovement>().BeginMovement(transform.parent.gameObject);
            }
            if (!isCut && Player.GetComponent<Equipment>().GetWeaponSlot() != null && Player.GetComponent<Equipment>().GetWeaponSlot().GetSpecificFunctions()["is_axe"]) {
                treeCutTime = true;
            }
            PlayerMovement.openGUI = false;
        }
    }

    public int GetGUIHeight() {
        return personalGUIHeight;
    }


    IEnumerator CutTree() {
        treeTime += 1;
        if (treeTime == 300) {
            Skills.skillList["Woodcutting"].IncreaseEXP(20);
            StopCoroutine(CutTree());
            treeCutTime = false;
            treeTime = 0;
            isCut = true;
        }
        yield return new WaitForSeconds(.1f);
    }

    public void InteractWith() {
        if (!isCut && Player.GetComponent<Equipment>().GetWeaponSlot() != null && Player.GetComponent<Equipment>().GetWeaponSlot().GetSpecificFunctions().ContainsKey("is_axe"))  {
            treeCutTime = true;
        }
    }
    public void StopCuttingTree() {
        treeCutTime = false;
        treeTime = 0;

    }
}