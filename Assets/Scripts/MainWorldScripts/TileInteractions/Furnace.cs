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
    string itemSmelting;
    Coroutine cor;
    GameObject Player;
    static Dictionary<string, GameObject> smeltableItems;
    readonly int guiHeight = 150;
    
    void Start() {
        smelting = false;
        smeltableItems = new();
        PlayerMovement.ResetActions += StopInteraction;
        Player = GameObject.Find("Player");
    }

    public void CreateOptions(float previousHeight) {
        GameObject interactionContainer = GameObject.Find("Interaction Container");
        
        GameObject interactButton = Instantiate(Resources.Load<GameObject>("UI/Interaction Menu Button"), interactionContainer.transform);
        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "placeholder";
        interactButton.GetComponent<Button>().onClick.AddListener(() => GUIInteract());
        interactButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -previousHeight);

        interactionContainer.GetComponent<RectTransform>().sizeDelta = interactionContainer.GetComponent<RectTransform>().sizeDelta + new Vector2(0, interactButton.GetComponent<RectTransform>().sizeDelta.y);
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
        if (smelted) {
            if (itemSmelting == "copper_ore") {
                Inventory.AddItem(Resources.Load<TextAsset>("Items/copper_bar").text);
            }
            Skills.skillList["Ignition"].IncreaseEXP(20);
            EXPGainPopup.CreateEXPGain("Ignition", 20, Skills.skillList["Ignition"].GetEXP() + 20, Skills.skillList["Ignition"].GetThreshold());
            yield break;
        } else if (smelting) {
            PopupManager.AddPopup("Wait", "Furnace is still smelting!"); 
            yield break;
        } 
        foreach (GameObject item in smeltableItems.Values) {
            Destroy(item);
        }
        smeltableItems = new();
        foreach (Item item in Inventory.inventoryList[2]) {
            if (item.GetSpecificFunctions().ContainsKey("smeltable")) {
                GameObject smeltableItem;
                if (!smeltableItems.Keys.Contains(item.GetName())) {
                    smeltableItem = Instantiate(Resources.Load<GameObject>("UI/Inventory Item"), GameObject.Find("Smeltable Items Container").transform);
                    smeltableItems[item.GetName()] = smeltableItem;
                    smeltableItem.GetComponentInChildren<TextMeshProUGUI>().text = "1";
                    smeltableItem.GetComponent<MouseOverItem>().SetItem(item);
                    smeltableItem.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(StartSmelting(item.GetName())));
                    smeltableItem.GetComponent<Button>().enabled = true;
                    smeltableItem.transform.Find("Item Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/" + item.GetName());
                } else {
                    smeltableItem = smeltableItems[item.GetName()];
                    smeltableItem.GetComponentInChildren<TextMeshProUGUI>().text =  int.Parse(smeltableItem.GetComponentInChildren<TextMeshProUGUI>().text) + 1 + "";
                }
            }
        }
        foreach (GameObject inventoryItem in smeltableItems.Values) {
            inventoryItem.GetComponentInChildren<TextMeshProUGUI>().text = inventoryItem.GetComponentInChildren<TextMeshProUGUI>().text + "x";
        }
        GameObject.Find("Furnace Canvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("Inventory and Skill Button Canvas").GetComponent<Canvas>().enabled = false;
    }
    IEnumerator StartSmelting(string item) {
        StopInteraction();
        smelting = true;
        itemSmelting = item;
        while (interactTime < 30) {
            interactTime++;
            yield return new WaitForSeconds(0.1f);
        }
        smelted = true;
    }

    public void InteractWith() {
        cor = StartCoroutine(StartInteraction());
    }
    public void StopInteraction() {
        if (cor != null) {
            StopCoroutine(cor);
        }
        GameObject.Find("Furnace Canvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Inventory and Skill Button Canvas").GetComponent<Canvas>().enabled = true;
    }

}