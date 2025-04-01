using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreeObject : MonoBehaviour, InteractableObject
{
    private readonly int personalGUIHeight = 50;
    private int interactTime;
    public bool isInteracted;
    Coroutine cor;
    GameObject Player;
    
    void Start() {
        isInteracted = false;
        cor = null;
        interactTime = 0;
        PlayerMovement.ResetActions += StopInteraction;
        Player = GameObject.Find("Player");
    }

    public void CreateOptions(float previousHeight) {
        
        GameObject interactionContainer = GameObject.Find("Interaction Container");
        
        GameObject interactButton = GameObject.Instantiate(Resources.Load<GameObject>("UI/Interaction Menu Button"), interactionContainer.transform);
        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Chop Tree";
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
        GameObject.Find("player model").transform.LookAt(transform);
        if (Equipment.GetEquippedItems()["Weapon Slot"] == null || !Equipment.GetEquippedItems()["Weapon Slot"].GetSpecificFunctions().ContainsKey("is_axe")) {
            PopupManager.AddPopup("Missing", "You need to equip an axe!");
            StopInteraction();
            yield break;
        }
        AnimationPlayerController.StartOneHandedSwingingAnimation();
        while (interactTime != 30) {
            interactTime += 1;
            yield return new WaitForSeconds(.1f);
        }
        foreach (Transform model in transform) {
            Destroy(model.gameObject);
        }
        AnimationPlayerController.EndOneHandedSwingingAnimation();
        Instantiate(Resources.Load<GameObject>("DecorationModels/treestump"), transform);
        Inventory.AddItem(Resources.Load<TextAsset>("Items/oak_log").text);
        Skills.skillList["Woodcutting"].IncreaseEXP(20);
        EXPGainPopup.CreateEXPGain("Woodcutting", 20, Skills.skillList["Woodcutting"].GetEXP(), Skills.skillList["Woodcutting"].GetThreshold());
        interactTime = 0;
        isInteracted = true;
    }

    public void InteractWith() {
        if (!isInteracted)  {
            cor = StartCoroutine(StartInteraction());
        }
    }
    public void StopInteraction() {
        if (cor != null) {
            StopCoroutine(cor);
            AnimationPlayerController.EndOneHandedSwingingAnimation();
        }
        interactTime = 0;

    }
}