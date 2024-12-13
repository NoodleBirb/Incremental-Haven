using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : MonoBehaviour, InteractableObject
{
    private readonly int personalGUIHeight = 50;
    private int waterTime;
    private bool fishingTime;
    public bool isFished;
    GameObject Player;
    
    void Start() {
        isFished = false;
        waterTime = 0;
        fishingTime = false;
        PlayerMovement.ResetActions += StopCuttingTree;
        Player = GameObject.Find("Player");
    }

    void Update() {
        if (fishingTime && Player.GetComponent<PlayerMovement>().movementPath.Count == 0) {
            StartCoroutine(Fish());
        }
    }

    public void CreateOptions(int previousHeight, Vector2 clickPos, int totalGUIWidth) {
        if (GUI.Button(new Rect(clickPos.x, Screen.height - clickPos.y + previousHeight, totalGUIWidth, personalGUIHeight), "Fish")) {
            Vector2Int pos = GetComponentInParent<BasicTile>().pos;
            if (Vector3.Distance(Player.transform.position, new(pos.x, 0, pos.y)) > 1){
                Player.GetComponent<PlayerMovement>().BeginMovement(transform.parent.gameObject);
            }
            if (!isFished && Player.GetComponent<Equipment>().GetWeaponSlot() != null && Player.GetComponent<Equipment>().GetWeaponSlot().GetSpecificFunctions()["is_fishing_rod"]) {
                fishingTime = true;
            }
            PlayerMovement.openGUI = false;
        }
    }

    public int GetGUIHeight() {
        return personalGUIHeight;
    }


    IEnumerator Fish() {
        waterTime += 1;
        if (waterTime == 300) {
            Skills.skillList["Fishing"].IncreaseEXP(20);
            StopCoroutine(Fish());
            fishingTime = false;
            waterTime = 0;
            isFished = true;
        }
        yield return new WaitForSeconds(.1f);
    }

    public void InteractWith() {
        if (!isFished && Player.GetComponent<Equipment>().GetWeaponSlot() != null && Player.GetComponent<Equipment>().GetWeaponSlot().GetSpecificFunctions().ContainsKey("is_fishing_rod"))  {
            fishingTime = true;
        }
    }
    public void StopCuttingTree() {
        fishingTime = false;
        waterTime = 0;

    }
}