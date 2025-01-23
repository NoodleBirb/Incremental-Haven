using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Defective.JSON;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    public static bool showInventory = false;
    public Rect openInventoryRect;
    public bool stillNotCloseEnough;
    public static bool shifting;
    GameObject player;
    GameObject mainCamera;
    public static event Action OnInventoryInitialized;
    public static bool isInventoryInitialized = false;
    public static List<Item> inventoryList;
    float startTime;
    Vector3 intitialPos;

    void Start() {
        startTime = Time.time;
        shifting = false;
        player = GameObject.Find("player model");
        mainCamera = GameObject.Find("Main Camera");
        intitialPos = mainCamera.transform.position;
        stillNotCloseEnough = false;
        showInventory = false;
        openInventoryRect = new(Screen.width - 200, Screen.height - 50, 100, 50);

        TextAsset stoneAxe = Resources.Load<TextAsset>("Items/stone_axe");
        Item item = LoadItemFromJson(stoneAxe.text);
        inventoryList ??= new() // eventually will be initialized with stuff from the saving system.
        {
                // Placeholder for testing
            item,
            item,
            item
        }; 
        isInventoryInitialized = true;
        OnInventoryInitialized?.Invoke();
    }
    public void OpenInventory() {
        showInventory = true;
        stillNotCloseEnough = true;
        PlayerMovement.openGUI = false;
        startTime = Time.time;
        intitialPos = mainCamera.transform.position;
        GameObject.Find("World Canvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Skill List Canvas").GetComponent<Canvas>().enabled = false;
        player.transform.LookAt(new Vector3(mainCamera.transform.position.x, 0, mainCamera.transform.position.z));
        StartCoroutine(ZoomIntoPlayer());
        // List the items. Coordinates begin in the corner of the ScrollView.
        
    }
    public void CloseInventory() {
        showInventory = false;
        GameObject.Find("World Canvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("Inventory Canvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Stats Canvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Equipment Canvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Skill List Canvas").GetComponent<Canvas>().enabled = false;
    }

    IEnumerator ZoomIntoPlayer() {

        if (Vector2.Distance(new(mainCamera.transform.position.x, mainCamera.transform.position.z), new(player.transform.position.x, player.transform.position.z)) <= 2.4f) {
            if (mainCamera.transform.position.y >= 1.5f) {
                mainCamera.transform.position = new (mainCamera.transform.position.x, mainCamera.transform.position.y - 3f * Time.deltaTime, mainCamera.transform.position.z);
                mainCamera.transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z));
                yield return null;
                StartCoroutine(ZoomIntoPlayer());
            } else {
                stillNotCloseEnough = false;
                shifting = true;
                StartCoroutine(ShiftCameraToRight());
                yield break;
            }
        }
        else {
            mainCamera.transform.position = Vector3.Lerp(intitialPos, new(player.transform.position.x, 1.5f, player.transform.position.z), Time.time - startTime);
            mainCamera.transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z));
            yield return null;
            StartCoroutine(ZoomIntoPlayer());
        }
    }
    IEnumerator ShiftCameraToRight() {
        Vector2 camPos = new(mainCamera.transform.position.x, mainCamera.transform.position.z);
        Vector2 playerPos = new(player.transform.position.x, player.transform.position.z);
        Vector3 rightConversion = new(mainCamera.transform.position.x + mainCamera.transform.right.x, mainCamera.transform.position.y, mainCamera.transform.position.z + mainCamera.transform.right.z);
        
        while (Vector2.Distance(camPos, playerPos) <= 2.7f) {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, rightConversion, 1.5f * Time.deltaTime);
            camPos = new(mainCamera.transform.position.x, mainCamera.transform.position.z);
            rightConversion = new(mainCamera.transform.position.x + mainCamera.transform.right.x, mainCamera.transform.position.y, mainCamera.transform.position.z + mainCamera.transform.right.z);
            yield return null;
        }
        shifting = false;
        LoadInventory();
        
    }

    public static void LoadInventory() {
        
        Transform content = GameObject.Find("Inventory List").transform;
        foreach (Transform transform in content) { 
            Destroy(transform.gameObject);         
        }   
        foreach (Item item in inventoryList) {
            GameObject equipmentItem = Instantiate(Resources.Load<GameObject>("UI/Equippable Item"));
            equipmentItem.GetComponent<MouseOverItem>().SetItem(item);
            if (item.GetSpecificFunctions()["weapon_slot"] == true) { // replace with a switch statement eventually
                equipmentItem.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => EquipItem(item, "Weapon Slot"));
            } else if (item.GetSpecificFunctions()["chestplate_slot"] == true) {
                equipmentItem.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => EquipItem(item, "Chestplate Slot"));
            } else if (item.GetSpecificFunctions()["headpiece_slot"] == true) {
                equipmentItem.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => EquipItem(item, "Head Piece Slot"));
            } else if (item.GetSpecificFunctions()["offhand_slot"] == true) {
                equipmentItem.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => EquipItem(item, "Off Hand Slot"));
            } else if (item.GetSpecificFunctions()["necklace_slot"] == true) {
                equipmentItem.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => EquipItem(item, "Necklace Slot"));
            } else if (item.GetSpecificFunctions()["leggings_slot"] == true) {
                equipmentItem.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => EquipItem(item, "Leggings Slot"));
            } else if (item.GetSpecificFunctions()["gloves_slot"] == true) {
                equipmentItem.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => EquipItem(item, "Glove Slot"));
            } else if (item.GetSpecificFunctions()["boots_slot"] == true) {
                equipmentItem.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => EquipItem(item, "Boots Slot"));
            }
            
            equipmentItem.transform.Find("Item Image").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("UI/Images/" + item.GetName());
            equipmentItem.transform.SetParent(GameObject.Find("Inventory List").transform);
        }
        PlayerStatistics.UpdateInventoryStats();
        GameObject.Find("Inventory Canvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("Stats Canvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("Equipment Canvas").GetComponent<Canvas>().enabled = true;
    }

    static void EquipItem(Item item, string itemType) {
        Item previouslyEquippedItem = Equipment.GetEquippedItems()[itemType];
        GameObject.Find(itemType).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => Equipment.SwapEquipmentPiece(null, itemType));
        GameObject.Find(itemType).GetComponent<UnityEngine.UI.Button>().interactable = true;
        GameObject.Find(itemType).GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("UI/Images/" + item.GetName());
        GameObject.Find(itemType).GetComponent<MouseOverItem>().SetItem(item);
        inventoryList.Remove(item);
        if (previouslyEquippedItem != null) {
            inventoryList.Add(previouslyEquippedItem);
        }
        Equipment.GetEquippedItems()[itemType] = item;
        MouseOverItem.ItemVanished();
        PlayerStatistics.UpdateStats();
        LoadInventory();
    }

    private static Item LoadItemFromJson(string json) {
        // Create JSONObject to easily sort through the data
        JSONObject obj = new(json);
        

        // Fill the stats dictionary with necessary values from the JSON file.
        Dictionary<string, float> statsDict;

        int i = 0;
        if (obj["equippable"].boolValue) {
            statsDict = new();
            var stats = obj["stats"];
            foreach (string val in stats.keys) {
                statsDict[val] = obj["stats"][i].floatValue;
                i++;
            }
        } else {
            statsDict = null;
        }
        
        Dictionary<string, bool> specificFunctionDict = new();
        var functions = obj["specific_functions"];
        i = 0;
        foreach (string str in functions.keys) {
            specificFunctionDict[str] = obj["specific_functions"][i].boolValue;
            i++;
        }

        return new Item(obj["name"].stringValue, obj["id"].intValue, obj["equippable"].boolValue, statsDict, specificFunctionDict, obj["description"].stringValue);
    }

    public static void AddItem(Item newItem) {
        inventoryList.Add(newItem);
    }
    public static void AddItem(string json) {
        inventoryList.Add(LoadItemFromJson(json));
    }
}