using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public void CreateOptions(float previousHeight) {
        
        GameObject interactionContainer = GameObject.Find("Interaction Container");
        
        GameObject interactButton = GameObject.Instantiate(Resources.Load<GameObject>("UI/Interaction Menu Button"));
        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Chop Tree";
        interactButton.GetComponent<Button>().onClick.AddListener(() => ChopTree());
        interactButton.transform.SetParent(interactionContainer.transform);
        interactButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -previousHeight);

        interactionContainer.GetComponent<RectTransform>().sizeDelta = interactionContainer.GetComponent<RectTransform>().sizeDelta + new Vector2(0, interactButton.GetComponent<RectTransform>().sizeDelta.y);
    }

    void ChopTree() {
        InteractableObject.ResetGUI();
        Vector2Int pos = GetComponentInParent<BasicTile>().pos;
        if (Vector3.Distance(Player.transform.position, new(pos.x, 0, pos.y)) > 1){
            Player.GetComponent<PlayerMovement>().BeginMovement(transform.parent.gameObject);
        }
        if (!isCut && Equipment.GetEquippedItems()["Weapon Slot"] != null && Equipment.GetEquippedItems()["Weapon Slot"].GetSpecificFunctions()["is_axe"]) {
            treeCutTime = true;
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
        if (!isCut && Equipment.GetEquippedItems()["Weapon Slot"] != null && Equipment.GetEquippedItems()["Weapon Slot"].GetSpecificFunctions().ContainsKey("is_axe"))  {
            treeCutTime = true;
        }
    }
    public void StopCuttingTree() {
        treeCutTime = false;
        treeTime = 0;

    }
}