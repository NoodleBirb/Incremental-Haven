using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Furnace : MonoBehaviour, InteractableObject
{
    private int interactTime;
    public bool smelted;
    bool smelting;
    bool fueling;
    bool attemptSmelting;
    bool pickingUp;
    string action;
    string itemSmelting;
    int burnTime;
    Coroutine cor;
    Coroutine smeltingCor;
    GameObject Player;
    static Dictionary<string, GameObject> smeltableItems;
    readonly int guiHeight = 150;
    
    void Start() {
        smelting = false;
        smeltableItems = new();
        PlayerMovement.ResetActions += StopInteraction;
        Player = GameObject.Find("Player");
    }

    void Update() {
        if (burnTime > 0 && smeltingCor == null) {
            smeltingCor = StartCoroutine(StartFueling());
        }
    }

    public void CreateOptions(float previousHeight) {
        GameObject interactionContainer = GameObject.Find("Interaction Container");
        
        GameObject interactButton = Instantiate(Resources.Load<GameObject>("UI/Interaction Menu Button"), interactionContainer.transform);
        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Add Fuel";
        interactButton.GetComponent<Button>().onClick.AddListener(() => {
            action = "fueling";
            GUIInteract();
        });
        
        interactButton = Instantiate(Resources.Load<GameObject>("UI/Interaction Menu Button"), interactionContainer.transform);
        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Smelt Item";
        interactButton.GetComponent<Button>().onClick.AddListener(() => {
            action = "attemptSmelting";
            GUIInteract();
        });
        
        interactButton = Instantiate(Resources.Load<GameObject>("UI/Interaction Menu Button"), interactionContainer.transform);
        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Pickup Item";
        interactButton.GetComponent<Button>().onClick.AddListener(() => {
            action = "pickingUp";
            GUIInteract();
        });
    }

    public int GetGUIHeight() {
        return guiHeight;
    }
    void GUIInteract() {
        InteractableObject.ResetGUI();
        Player.GetComponent<PlayerMovement>().BeginMovement(transform.parent.gameObject);
    }

    IEnumerator StartInteraction() {
        while (Player.GetComponent<PlayerMovement>().movementPath.Count != 0) {
            yield return null;
        }
        GameObject.Find("player model").transform.LookAt(transform);
        if (!fueling && !attemptSmelting) {
            if (smelted) {
                switch (itemSmelting) {
                    case "copper_ore":
                        Inventory.AddItem(Resources.Load<TextAsset>("Items/copper_bar").text);
                        break;
                    case "cod":
                        Inventory.AddItem(Resources.Load<TextAsset>("Items/cooked_cod").text);
                        break;
                }
                AnimationPlayerController.PutAway();
                Skills.skillList["Ignition"].IncreaseEXP(20);
                EXPGainPopup.CreateEXPGain("Ignition", 20, Skills.skillList["Ignition"].GetEXP() + 20, Skills.skillList["Ignition"].GetThreshold());
                smelted = false;

                itemSmelting = null;
                yield break;
            } else if (smelting && burnTime == 0) {
                PopupManager.AddPopup("Add", "Furnace needs fuel!"); 
                yield break;
            } else if (smelting && burnTime != 0) {
                PopupManager.AddPopup("Wait", "Furnace is still smelting!"); 
                yield break;
            } else if (pickingUp) {
                PopupManager.AddPopup("Add", "No ore is smelting"); 
                yield break;
            }
        }
        foreach (GameObject item in smeltableItems.Values) {
            Destroy(item);
        }
        smeltableItems = new(); 
        if (attemptSmelting) {
            if (smelting) {
                if (burnTime == 0) {
                    PopupManager.AddPopup("Add", "Furnace needs fuel!"); 
                } else {
                    PopupManager.AddPopup("Wait", "Furnace is still smelting!"); 
                }
                yield break;
            } else if (smelted) {
                PopupManager.AddPopup("Pick Up", "Pick up your smelted resource!"); 
                yield break;
            }
        }
        foreach (Item item in Inventory.inventoryList[2]) {
            GameObject smeltableItem;
            if (!smeltableItems.Keys.Contains(item.GetName())) {
                if (((burnTime == 0 && !attemptSmelting) || fueling) && item.GetSpecificFunctions().ContainsKey("fuel")) {
                    smeltableItem = Instantiate(Resources.Load<GameObject>("UI/Inventory Item"), GameObject.Find("Smeltable Items Container").transform);
                    smeltableItems[item.GetName()] = smeltableItem;
                    smeltableItem.GetComponentInChildren<TextMeshProUGUI>().text = "1";
                    smeltableItem.GetComponent<MouseOverItem>().SetItem(item);
                    smeltableItem.GetComponent<Button>().onClick.AddListener(() => AddFuel(item));
                    smeltableItem.GetComponent<Button>().enabled = true;
                    smeltableItem.transform.Find("Item Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/" + item.GetName());
                }
                else if (((burnTime > 0 && !fueling) || attemptSmelting) && item.GetSpecificFunctions().ContainsKey("smeltable")) {
                    smeltableItem = Instantiate(Resources.Load<GameObject>("UI/Inventory Item"), GameObject.Find("Smeltable Items Container").transform);
                    smeltableItems[item.GetName()] = smeltableItem;
                    smeltableItem.GetComponentInChildren<TextMeshProUGUI>().text = "1";
                    smeltableItem.GetComponent<MouseOverItem>().SetItem(item);
                    smeltableItem.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(StartSmelting(item)));
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
        fueling = false;
        attemptSmelting = false;
        GameObject.Find("Furnace Canvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("Inventory and Skill Button Canvas").GetComponent<Canvas>().enabled = false;
    }

    void AddFuel(Item item) {
        StopInteraction();
        AnimationPlayerController.PutAway();
        Inventory.inventoryList[2].Remove(item);
        Skills.skillList["Ignition"].IncreaseEXP(4);
        EXPGainPopup.CreateEXPGain("Ignition", 4, Skills.skillList["Ignition"].GetEXP(), Skills.skillList["Ignition"].GetThreshold());
        if (item.GetName() == "oak_log") {
            burnTime += 30;
        }
    }
    
    IEnumerator StartSmelting(Item item) {
        Debug.Log("im starting to smelt");
        StopInteraction();
        AnimationPlayerController.PutAway();
        Inventory.inventoryList[2].Remove(item);
        smelting = true;
        interactTime = 0;
        itemSmelting = item.GetName();
        while (interactTime < 30) {
            while (burnTime == 0) {
                yield return null;
            }
            interactTime++;
            yield return new WaitForSeconds(0.1f);
        }
        smelting = false;
        smelted = true;
    }

    IEnumerator StartFueling() {
        while (burnTime > 0) {
            burnTime--;
            yield return new WaitForSeconds(1f);
        }
        smeltingCor = null;
    }

    public void InteractWith() {
        if (action == "fueling") {
            fueling = true;
            action = null;
        } else if (action == "attemptSmelting") {
            attemptSmelting = true;
            action = null;
        } else if (action == "pickingUp") {
            pickingUp = true;
            action = null;
        }
        cor = StartCoroutine(StartInteraction());
    }
    public void StopInteraction() {
        if (cor != null) {
            StopCoroutine(cor);
        }
        fueling = false;
        attemptSmelting = false;
        GameObject.Find("Furnace Canvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Inventory and Skill Button Canvas").GetComponent<Canvas>().enabled = true;
    }
}