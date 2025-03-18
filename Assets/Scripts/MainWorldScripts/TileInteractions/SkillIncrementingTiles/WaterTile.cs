using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaterTile : MonoBehaviour, InteractableObject
{
    private readonly int personalGUIHeight = 50;
    private int interactTime;
    public bool isInteracted;
    Coroutine cor;
    GameObject Player;
    
    void Start() {
        isInteracted = false;
        interactTime = 0;
        cor = null;
        PlayerMovement.ResetActions += StopInteraction;
        Player = GameObject.Find("Player");
    }

    public void CreateOptions(float previousHeight) {
        GameObject interactionContainer = GameObject.Find("Interaction Container");
        
        GameObject interactButton = Instantiate(Resources.Load<GameObject>("UI/Interaction Menu Button"), interactionContainer.transform);
        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Fish";
        interactButton.GetComponent<Button>().onClick.AddListener(() => GUIInteract());
    }

    void GUIInteract() {
        InteractableObject.ResetGUI();
        Player.GetComponent<PlayerMovement>().BeginMovement(transform.parent.gameObject);
    }

    public int GetGUIHeight() {
        return personalGUIHeight;
    }


    IEnumerator StartInteraction() {
        while (Player.GetComponent<PlayerMovement>().movementPath.Count != 0) {
            yield return null;
        }
        if (Equipment.GetEquippedItems()["Weapon Slot"] == null || !Equipment.GetEquippedItems()["Weapon Slot"].GetSpecificFunctions().ContainsKey("is_fishing_rod")) {
            PopupManager.AddPopup("Missing", "You need to equip a fishing rod!");
            StopInteraction();
            yield break;
        }
        while (interactTime != 30) {
            interactTime += 1;
            yield return new WaitForSeconds(.1f);
        }
        gameObject.transform.parent.Find("Plane").GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Materials/WaterTileTexture");
        Inventory.AddItem(Resources.Load<TextAsset>("Items/cod").text);
        Skills.skillList["Fishing"].IncreaseEXP(20);
        EXPGainPopup.CreateEXPGain("Fishing", 20, Skills.skillList["Fishing"].GetEXP(), Skills.skillList["Fishing"].GetThreshold());
        interactTime = 0;
        isInteracted = true;
    }

    public void InteractWith() {
        if (!isInteracted)  {
            cor = StartCoroutine(StartInteraction());
        } else {
            PopupManager.AddPopup("Empty", "No fish left here");
        }
    }
    public void StopInteraction() {
        if (cor != null) {
            StopCoroutine(cor);
        }
        cor = null;
        interactTime = 0;
    }
}