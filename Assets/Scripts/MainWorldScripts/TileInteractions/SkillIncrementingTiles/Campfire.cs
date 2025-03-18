using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Campfire : MonoBehaviour, InteractableObject
{
    private readonly int personalGUIHeight = 50;
    int burnTime;
    int tickTime;
    GameObject Player;
    GameObject baseTile;
    bool fueling;
    Coroutine tickCor;
    Coroutine interactCor;
    bool playerBurning;
    static Dictionary<string, GameObject> smeltableItems;
    
    void Start() {
        burnTime = 0;
        Player = GameObject.Find("Player");
        PlayerMovement.ResetActions += StopInteraction;
        baseTile = gameObject.transform.parent.gameObject;
        while (baseTile.GetComponent<TileSettings>() == null || (baseTile.GetComponent<TileSettings>() != null && !baseTile.GetComponent<TileSettings>().baseTile)) {
            baseTile = baseTile.transform.parent.gameObject;
        }
        fueling = false;
        baseTile.GetComponent<TileSettings>().walkable = false;
        smeltableItems = new();
    }

    void Update() {
        if (burnTime > 0 && Player.GetComponent<PlayerMovement>().GetTechnicalPos() == new Vector2Int((int)baseTile.transform.position.x, (int)baseTile.transform.position.z)) {
            playerBurning = true;
            tickCor ??= StartCoroutine(FireTick());
        } else {
            tickCor = null;
            playerBurning = false;
        }
    }

    public void InteractWith() {
        interactCor = StartCoroutine (StartInteraction());
    }

    IEnumerator StartInteraction() {
        while (Player.GetComponent<PlayerMovement>().movementPath.Count != 0) {
            yield return null;
        }
        if (burnTime == 0) {
            foreach (GameObject item in smeltableItems.Values) {
                Destroy(item);
            }
            smeltableItems = new();
            foreach (Item item in Inventory.inventoryList[2]) {
                GameObject smeltableItem;
                if (!smeltableItems.Keys.Contains(item.GetName())) {
                    if (item.GetSpecificFunctions().ContainsKey("fuel")) {
                        smeltableItem = Instantiate(Resources.Load<GameObject>("UI/Inventory Item"), GameObject.Find("Campfire Burnable Items Container").transform);
                        smeltableItems[item.GetName()] = smeltableItem;
                        smeltableItem.GetComponentInChildren<TextMeshProUGUI>().text = "1";
                        smeltableItem.GetComponent<MouseOverItem>().SetItem(item);
                        smeltableItem.GetComponent<Button>().onClick.AddListener(() => AddFuel(item));
                        smeltableItem.GetComponent<Button>().enabled = true;
                        smeltableItem.transform.Find("Item Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/" + item.GetName());
                    }
                } else {
                    smeltableItem = smeltableItems[item.GetName()];
                    smeltableItem.GetComponentInChildren<TextMeshProUGUI>().text =  int.Parse(smeltableItem.GetComponentInChildren<TextMeshProUGUI>().text) + 1 + "";
                }
            }
            foreach (GameObject inventoryItem in smeltableItems.Values) {
                inventoryItem.GetComponentInChildren<TextMeshProUGUI>().text = inventoryItem.GetComponentInChildren<TextMeshProUGUI>().text + "x";
            }
            GameObject.Find("Campfire Canvas").GetComponent<Canvas>().enabled = true;
            GameObject.Find("Inventory and Skill Button Canvas").GetComponent<Canvas>().enabled = false;
        } else {
            baseTile.GetComponent<TileSettings>().walkable = false;
        }
    }
    public void StopInteraction() {
        if (interactCor != null) {
            StopCoroutine(interactCor);
            interactCor = null;
        }
        GameObject.Find("Campfire Canvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Inventory and Skill Button Canvas").GetComponent<Canvas>().enabled = true;
    }

    public void CreateOptions(float previousHeight) {
        GameObject interactionContainer = GameObject.Find("Interaction Container");
        
        GameObject interactButton = Instantiate(Resources.Load<GameObject>("UI/Interaction Menu Button"), interactionContainer.transform);
        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Add Fuel";
        interactButton.GetComponent<Button>().onClick.AddListener(() => GUIInteract());
    }

    void GUIInteract() {
        InteractableObject.ResetGUI();
        Player.GetComponent<PlayerMovement>().BeginMovement(baseTile, campfire: true);
    }

    public int GetGUIHeight() {
        return personalGUIHeight;
    }

    IEnumerator FireTick() {
        tickTime = 0;
        while (playerBurning) {
            tickTime++;
            if (tickTime == 30) {
                Skills.skillList["Endurance"].IncreaseEXP(20);
                EXPGainPopup.CreateEXPGain("Endurance", 20, Skills.skillList["Endurance"].GetEXP(), Skills.skillList["Endurance"].GetThreshold());
                PlayerStatistics.currentHP -= PlayerStatistics.totalStats["HP"] * 0.1f;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void AddFuel(Item item) {
        Inventory.inventoryList[2].Remove(item);
        baseTile.GetComponent<TileSettings>().walkable = true;
        Skills.skillList["Ignition"].IncreaseEXP(4);
        EXPGainPopup.CreateEXPGain("Ignition", 4, Skills.skillList["Ignition"].GetEXP(), Skills.skillList["Ignition"].GetThreshold());
        if (item.GetName() == "oak_log") {
            burnTime += 30;
        }
        StopInteraction();
    }
}