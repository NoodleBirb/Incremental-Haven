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
    public bool isCut;
    Coroutine cor;
    GameObject Player;
    
    void Start() {
        isCut = false;
        cor = null;
        treeTime = 0;
        PlayerMovement.ResetActions += StopCuttingTree;
        Player = GameObject.Find("Player");
    }

    public void CreateOptions(float previousHeight) {
        
        GameObject interactionContainer = GameObject.Find("Interaction Container");
        
        GameObject interactButton = GameObject.Instantiate(Resources.Load<GameObject>("UI/Interaction Menu Button"), interactionContainer.transform);
        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Chop Tree";
        interactButton.GetComponent<Button>().onClick.AddListener(() => ChopTree());
        interactButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -previousHeight);

        interactionContainer.GetComponent<RectTransform>().sizeDelta = interactionContainer.GetComponent<RectTransform>().sizeDelta + new Vector2(0, interactButton.GetComponent<RectTransform>().sizeDelta.y);
    }

    void ChopTree() {
        InteractableObject.ResetGUI();
        Vector2Int pos = GetComponentInParent<BasicTile>().pos;
        if (Vector3.Distance(Player.transform.position, new(pos.x, 0, pos.y)) > 1){
            Player.GetComponent<PlayerMovement>().BeginInteractionMovement(transform.parent.gameObject);
        }
        if (!isCut && Equipment.GetEquippedItems()["Weapon Slot"] != null && Equipment.GetEquippedItems()["Weapon Slot"].GetSpecificFunctions()["is_axe"]) {
            Debug.Log("I should be running only once");
            cor = StartCoroutine(CutTree());
        }
    }

    public int GetGUIHeight() {
        return personalGUIHeight;
    }


    IEnumerator CutTree() {
        
        while (Player.GetComponent<PlayerMovement>().movementPath.Count != 0) {
            
            yield return null;
        }
        while (treeTime != 30) {
            treeTime += 1;
            yield return new WaitForSeconds(.1f);
        }
        EXPGainPopup.CreateEXPGain("Woodcutting", 20, (int)Skills.skillList["Woodcutting"].GetEXP() + 20, (int)Skills.skillList["Woodcutting"].GetThreshold());
        Skills.skillList["Woodcutting"].IncreaseEXP(20);
        StopCoroutine(cor);
        treeTime = 0;
        isCut = true;
    }

    public void InteractWith() {
        if (!isCut && Equipment.GetEquippedItems()["Weapon Slot"] != null && Equipment.GetEquippedItems()["Weapon Slot"].GetSpecificFunctions().ContainsKey("is_axe"))  {
            cor = StartCoroutine(CutTree());
        }
    }
    public void StopCuttingTree() {
        if (cor != null) {
            StopCoroutine(cor);
        }
        treeTime = 0;

    }
}