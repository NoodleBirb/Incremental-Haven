using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ore : MonoBehaviour, InteractableObject
{
    private readonly int personalGUIHeight = 50;
    private int interactTime;
    public bool isInteracted;
    Coroutine cor;
    GameObject Player;
    Renderer oreRenderer;
    public string typeOre = null;
    
    void Start() {
        isInteracted = false;
        cor = null;
        interactTime = 0;
        PlayerMovement.ResetActions += StopInteraction;
        Player = GameObject.Find("Player");
        oreRenderer = transform.Find("Ore").gameObject.GetComponent<Renderer>();
        Material[] mats = oreRenderer.materials;
        if (typeOre != null) {
            for (int i = 0; i < mats.Length; i++) {
                mats[i] = Resources.Load<Material>("Materials/Materials/" + typeOre);
            }
            oreRenderer.materials = mats;
        }
    }

    public void CreateOptions(float previousHeight) {
        
        GameObject interactionContainer = GameObject.Find("Interaction Container");
        
        GameObject interactButton = GameObject.Instantiate(Resources.Load<GameObject>("UI/Interaction Menu Button"), interactionContainer.transform);
        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Mine";
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
        if (Equipment.GetEquippedItems()["Weapon Slot"] == null || !Equipment.GetEquippedItems()["Weapon Slot"].GetSpecificFunctions().ContainsKey("is_pickaxe")) {
            PopupManager.AddPopup("Missing", "You need to equip a pickaxe!");
            StopInteraction();
            yield break;
        }
        AnimationPlayerController.StartOneHandedSwingingAnimation();
        while (interactTime != 30) {
            while (Inventory.fullyOpen) {
                yield return null;
            }
            interactTime += 1;
            yield return new WaitForSeconds(.1f);
        }
        AnimationPlayerController.EndOneHandedSwingingAnimation();
        Inventory.AddItem(Resources.Load<TextAsset>("Items/copper_ore").text);
        Skills.skillList["Mining"].IncreaseEXP(20);
        EXPGainPopup.CreateEXPGain("Mining", 20, Skills.skillList["Mining"].GetEXP(), Skills.skillList["Mining"].GetThreshold());
        interactTime = 0;
        isInteracted = true;
        Material[] mats = oreRenderer.materials;
        if (typeOre != null) {
            for (int i = 0; i < mats.Length; i++) {
                mats[i] = Resources.Load<Material>("Materials/Materials/stone");
            }
            oreRenderer.materials = mats;
        }
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