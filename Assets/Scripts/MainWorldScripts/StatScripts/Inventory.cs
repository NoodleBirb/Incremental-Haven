using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Defective.JSON;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static bool showInventory = false;
    public bool stillNotCloseEnough;
    public static bool shifting;
    GameObject player;
    GameObject mainCamera;
    public static List<List<Item>> inventoryList;
    static Dictionary<Item, GameObject> currentInventoryItems;
    float startTime;
    Vector3 intitialPos;
    public static bool fullyOpen;
    public static int tab = 0;

    void Awake() {
        startTime = Time.time;
        shifting = false;
        stillNotCloseEnough = false;
        showInventory = false;
        fullyOpen = false;

        currentInventoryItems = new();
        
        inventoryList ??= new(3) { // eventually will be initialized with stuff from the saving system.
            new List<Item>(),
            new List<Item>(),
            new List<Item>()
        };
    }
    void Start() {
        player = GameObject.Find("player model");
        mainCamera = GameObject.Find("Main Camera");
        intitialPos = mainCamera.transform.position;

        TextAsset stoneAxe = Resources.Load<TextAsset>("Items/stone_axe");
        Item item = LoadItemFromJson(stoneAxe.text);
        inventoryList[0].Add(item);
        inventoryList[1].Add(LoadItemFromJson(Resources.Load<TextAsset>("Items/basic_health_potion").text));
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
        fullyOpen = false;
        Equipment.DisableLines();
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
        currentInventoryItems = new ();
        foreach (Transform transform in content) { 
            Destroy(transform.gameObject);
        }   
        
        foreach (Item item in inventoryList[tab]) {
            if (tab == 0) {
                GameObject equipmentItem;
                if (!currentInventoryItems.Keys.Contains(item)) {
                    equipmentItem = Instantiate(Resources.Load<GameObject>("UI/Equippable Item"), GameObject.Find("Inventory List").transform);
                    currentInventoryItems[item] = equipmentItem;
                    equipmentItem.GetComponentInChildren<TextMeshProUGUI>().text = 1 + "";
                    equipmentItem.GetComponent<MouseOverItem>().SetItem(item);
                    if (item.GetSpecificFunctions()["weapon_slot"] == true) { // replace with a switch statement eventually
                        equipmentItem.GetComponent<Button>().onClick.AddListener(() => EquipItem(item, "Weapon Slot"));
                    } else if (item.GetSpecificFunctions()["chestplate_slot"] == true) {
                        equipmentItem.GetComponent<Button>().onClick.AddListener(() => EquipItem(item, "Chestplate Slot"));
                    } else if (item.GetSpecificFunctions()["headpiece_slot"] == true) {
                        equipmentItem.GetComponent<Button>().onClick.AddListener(() => EquipItem(item, "Head Piece Slot"));
                    } else if (item.GetSpecificFunctions()["offhand_slot"] == true) {
                        equipmentItem.GetComponent<Button>().onClick.AddListener(() => EquipItem(item, "Off Hand Slot"));
                    } else if (item.GetSpecificFunctions()["necklace_slot"] == true) {
                        equipmentItem.GetComponent<Button>().onClick.AddListener(() => EquipItem(item, "Necklace Slot"));
                    } else if (item.GetSpecificFunctions()["leggings_slot"] == true) {
                        equipmentItem.GetComponent<Button>().onClick.AddListener(() => EquipItem(item, "Leggings Slot"));
                    } else if (item.GetSpecificFunctions()["gloves_slot"] == true) {
                        equipmentItem.GetComponent<Button>().onClick.AddListener(() => EquipItem(item, "Glove Slot"));
                    } else if (item.GetSpecificFunctions()["boots_slot"] == true) {
                        equipmentItem.GetComponent<Button>().onClick.AddListener(() => EquipItem(item, "Boots Slot"));
                    }
                    equipmentItem.transform.Find("Item Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/" + item.GetName());
                } else {
                    equipmentItem = currentInventoryItems[item];
                    equipmentItem.GetComponentInChildren<TextMeshProUGUI>().text = int.Parse(equipmentItem.GetComponentInChildren<TextMeshProUGUI>().text) + 1 + "";
                }
            } else if (tab == 1) {
                GameObject consumableItem = Instantiate(Resources.Load<GameObject>("UI/Consumable Item"), GameObject.Find("Inventory List").transform);
                consumableItem.GetComponent<Button>().onClick.AddListener(() => Consumables.UseConsumable(item));
                consumableItem.transform.Find("Item Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/" + item.GetName());
            } else {
                GameObject inventoryItem = Instantiate(Resources.Load<GameObject>("UI/Inventory Item"), GameObject.Find("Inventory List").transform);
                inventoryItem.transform.Find("Item Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/" + item.GetName());
            }
        }
        PlayerStatistics.UpdateInventoryStats();
        GameObject.Find("Inventory Canvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("Stats Canvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("Equipment Canvas").GetComponent<Canvas>().enabled = true;
        Equipment.EnableLines();
        fullyOpen = true;
    }

    public static void OpenTab(int tab) {
        Inventory.tab = tab;
        LoadInventory();
    }

    static void EquipItem(Item item, string itemType) {
        Item previouslyEquippedItem = Equipment.GetEquippedItems()[itemType];
        GameObject.Find(itemType).GetComponent<Button>().onClick.AddListener(() => EquipItem(null, itemType));
        GameObject.Find(itemType).GetComponent<Button>().interactable = true;
        if (item == null) {
            GameObject.Find(itemType).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/transparentbackground");
            if (itemType == "Weapon Slot") {
                Skills.currentWeaponSkill = Skills.skillList["MakeshiftCombat"];
            }
        } else {
            GameObject.Find(itemType).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/" + item.GetName());
            if (itemType == "Weapon Slot") {
                if (item.GetSpecificFunctions().ContainsKey("one_handed")) {
                    Skills.currentWeaponSkill = Skills.skillList["OneHandedCombat"];
                } else if (item.GetSpecificFunctions().ContainsKey("two_handed")) {
                    Skills.currentWeaponSkill = Skills.skillList["TwoHandedCombat"];
                } else if (item.GetSpecificFunctions().ContainsKey("light_ranged")) {
                    Skills.currentWeaponSkill = Skills.skillList["LightRanged"];
                } else if (item.GetSpecificFunctions().ContainsKey("heavy_ranged")) {
                    Skills.currentWeaponSkill = Skills.skillList["HeavyRanged"];
                } else if (item.GetSpecificFunctions().ContainsKey("magic")) {
                    Skills.currentWeaponSkill = Skills.skillList["Magic"];
                }
            }
        }
        
        GameObject.Find(itemType).GetComponent<MouseOverItem>().SetItem(item);
        inventoryList[0].Remove(item);
        if (previouslyEquippedItem != null) {
            inventoryList[0].Add(previouslyEquippedItem);
        }
        Equipment.GetEquippedItems()[itemType] = item;
        MouseOverItem.ItemVanished();
        PlayerStatistics.UpdateStats();
        CreateVisualItem(item, itemType);
        LoadInventory();
    }

    static void CreateVisualItem(Item item, string type) {
        if (item == null) return;
        Debug.Log(item.GetName());
        if (type == "Weapon Slot") {
            GameObject equippedItem = Instantiate(Resources.Load<GameObject>("ItemModels/" + item.GetName()), GameObject.Find("Left Arm").transform);
        }
    }

    private static Item LoadItemFromJson(string json) {
        // Create JSONObject to easily sort through the data
        JSONObject obj = new(json);
        

        // Fill the stats dictionary with necessary values from the JSON file.
        Dictionary<string, float> statsDict;

        int i = 0;
        if (obj["equippable"].boolValue || obj["consumable"].boolValue) {
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

        return new Item(obj["name"].stringValue, obj["id"].intValue, obj["equippable"].boolValue, obj["consumable"].boolValue, obj["material"].boolValue, statsDict, specificFunctionDict, obj["description"].stringValue);
    }

    public static void AddItem(Item newItem) {
        if (newItem.IsEquippable()) {
            inventoryList[0].Add(newItem);
        } else if (newItem.IsConsumable()) {
            inventoryList[1].Add(newItem);
        }
    }
    public static void AddItem(string json) {
        Item newItem = LoadItemFromJson(json);
        if (newItem.IsEquippable()) {
            inventoryList[0].Add(newItem);
        } else if (newItem.IsConsumable()) {
            inventoryList[1].Add(newItem);
        }
    }
}