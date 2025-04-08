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
    static Dictionary<string, GameObject> currentInventoryItems;
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
        inventoryList[2].Add(LoadItemFromJson(Resources.Load<TextAsset>("Items/copper_ore").text));
        inventoryList[2].Add(LoadItemFromJson(Resources.Load<TextAsset>("Items/oak_log").text));
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
        AnimationPlayerController.PauseAnimations();
        StartCoroutine(ZoomIntoPlayer());
        // List the items. Coordinates begin in the corner of the ScrollView.
        
    }
    public void CloseInventory() {
        showInventory = false;
        fullyOpen = false;
        AnimationPlayerController.ResumeAnimations();
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

    public static void LoadCombatInventory() {
        Transform content = GameObject.Find("Inventory List").transform;
        currentInventoryItems = new ();
        foreach (Transform transform in content) { 
            Destroy(transform.gameObject);
        }   
        
        foreach (Item item in inventoryList[1]) {
            GameObject consumableItem;
            if (!currentInventoryItems.Keys.Contains(item.GetName())) {
                consumableItem = Instantiate(Resources.Load<GameObject>("UI/Inventory Item"), GameObject.Find("Inventory List").transform);
                currentInventoryItems[item.GetName()] = consumableItem;
                consumableItem.GetComponent<MouseOverItem>().SetItem(item);
                consumableItem.GetComponentInChildren<TextMeshProUGUI>().text = "1";
                consumableItem.GetComponent<Button>().onClick.AddListener(() => Consumables.UseCombatConsumable(item));
                consumableItem.transform.Find("Item Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/" + item.GetName());
            } else {
                consumableItem = currentInventoryItems[item.GetName()];
                consumableItem.GetComponentInChildren<TextMeshProUGUI>().text =  int.Parse(consumableItem.GetComponentInChildren<TextMeshProUGUI>().text) + 1 + "";
            }
        }
        foreach (GameObject inventoryItem in currentInventoryItems.Values) {
            inventoryItem.GetComponentInChildren<TextMeshProUGUI>().text = inventoryItem.GetComponentInChildren<TextMeshProUGUI>().text + "x";
        }
        PlayerStatistics.UpdateInventoryStats();
        GameObject.Find("Inventory Canvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("Stats Canvas").GetComponent<Canvas>().enabled = true;
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
                if (!currentInventoryItems.Keys.Contains(item.GetName())) {
                    equipmentItem = Instantiate(Resources.Load<GameObject>("UI/Inventory Item"), GameObject.Find("Inventory List").transform);
                    currentInventoryItems[item.GetName()] = equipmentItem;
                    equipmentItem.GetComponentInChildren<TextMeshProUGUI>().text = "1";
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
                    equipmentItem = currentInventoryItems[item.GetName()];
                    equipmentItem.GetComponentInChildren<TextMeshProUGUI>().text =  int.Parse(equipmentItem.GetComponentInChildren<TextMeshProUGUI>().text) + 1 + "";
                }
            } else if (tab == 1) {
                GameObject consumableItem;
                if (!currentInventoryItems.Keys.Contains(item.GetName())) {
                    consumableItem = Instantiate(Resources.Load<GameObject>("UI/Inventory Item"), GameObject.Find("Inventory List").transform);
                    currentInventoryItems[item.GetName()] = consumableItem;
                    consumableItem.GetComponent<MouseOverItem>().SetItem(item);
                    consumableItem.GetComponentInChildren<TextMeshProUGUI>().text = "1";
                    consumableItem.GetComponent<Button>().onClick.AddListener(() => Consumables.UseConsumable(item));
                    consumableItem.transform.Find("Item Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/" + item.GetName());
                } else {
                    consumableItem = currentInventoryItems[item.GetName()];
                    consumableItem.GetComponentInChildren<TextMeshProUGUI>().text =  int.Parse(consumableItem.GetComponentInChildren<TextMeshProUGUI>().text) + 1 + "";
                }
            } else {
                GameObject inventoryItem;
                if (!currentInventoryItems.Keys.Contains(item.GetName())) {
                    inventoryItem = Instantiate(Resources.Load<GameObject>("UI/Inventory Item"), GameObject.Find("Inventory List").transform);
                    currentInventoryItems[item.GetName()] = inventoryItem;
                    inventoryItem.GetComponentInChildren<TextMeshProUGUI>().text = "1";
                    inventoryItem.GetComponent<MouseOverItem>().SetItem(item);
                    inventoryItem.GetComponent<Button>().enabled = false;
                    inventoryItem.transform.Find("Item Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/" + item.GetName());
                } else {
                    inventoryItem = currentInventoryItems[item.GetName()];
                    inventoryItem.GetComponentInChildren<TextMeshProUGUI>().text =  int.Parse(inventoryItem.GetComponentInChildren<TextMeshProUGUI>().text) + 1 + "";
                }
            }
        }
        foreach (GameObject inventoryItem in currentInventoryItems.Values) {
            inventoryItem.GetComponentInChildren<TextMeshProUGUI>().text = inventoryItem.GetComponentInChildren<TextMeshProUGUI>().text + "x";
        }
        PlayerStatistics.UpdateInventoryStats();
        GameObject.Find("Inventory Canvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("Stats Canvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("Equipment Canvas").GetComponent<Canvas>().enabled = true;
        fullyOpen = true;
    }

    public static void OpenTab(int tab) {
        Inventory.tab = tab;
        LoadInventory();
    }

    static void EquipItem(Item item, string itemType) {
        Item previouslyEquippedItem = Equipment.GetEquippedItems()[itemType];
        if (previouslyEquippedItem != null && GameObject.Find(previouslyEquippedItem.GetName()) != null) {
            GameObject.Find(previouslyEquippedItem.GetName()).GetComponent<MeshRenderer>().enabled = false;
        }
        GameObject.Find(itemType).GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find(itemType).GetComponent<Button>().onClick.AddListener(() => EquipItem(null, itemType));
        GameObject.Find(itemType).GetComponent<Button>().interactable = true;
        if (item == null) {
            GameObject.Find(itemType).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/transparentbackground");
            GameObject.Find(itemType).GetComponent<Button>().interactable = false;
            if (itemType == "Weapon Slot") {
                Skills.ChangeWeaponSkill(Skills.skillList["MakeshiftCombat"]);
            }
        } else {
            if (GameObject.Find(item.GetName()) != null) {
                GameObject.Find(item.GetName()).GetComponent<MeshRenderer>().enabled = true;
            }
            GameObject.Find(itemType).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/" + item.GetName());
            if (itemType == "Weapon Slot") {
                if (item.GetSpecificFunctions().ContainsKey("one_handed")) {
                    Skills.ChangeWeaponSkill(Skills.skillList["OneHandedCombat"]);
                } else if (item.GetSpecificFunctions().ContainsKey("two_handed")) {
                    Skills.ChangeWeaponSkill(Skills.skillList["TwoHandedCombat"]);
                } else if (item.GetSpecificFunctions().ContainsKey("light_ranged")) {
                    Skills.ChangeWeaponSkill(Skills.skillList["LightRanged"]);
                } else if (item.GetSpecificFunctions().ContainsKey("heavy_ranged")) {
                    Skills.ChangeWeaponSkill(Skills.skillList["HeavyRanged"]);
                } else if (item.GetSpecificFunctions().ContainsKey("magic")) {
                    Skills.ChangeWeaponSkill(Skills.skillList["Magic"]);
                }
            }
        }
        
        GameObject.Find(itemType).GetComponent<MouseOverItem>().SetItem(item);
        if (item != null) {
            
            inventoryList[0].Remove(item);
        }
        if (previouslyEquippedItem != null) {
            inventoryList[0].Add(previouslyEquippedItem);
        }
        Equipment.GetEquippedItems()[itemType] = item;
        MouseOverItem.ItemVanished();
        PlayerStatistics.UpdateStats();
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
        if (!functions.isNull) {
            i = 0;
            foreach (string str in functions.keys) {
                specificFunctionDict[str] = obj["specific_functions"][i].boolValue;
                i++;
            }
        }

        return new Item(obj["name"].stringValue, obj["equippable"].boolValue, obj["consumable"].boolValue, obj["material"].boolValue, statsDict, specificFunctionDict, obj["description"].stringValue);
    }

    public static void AddItem(Item newItem) {
        if (newItem.IsEquippable()) {
            inventoryList[0].Add(newItem);
        } else if (newItem.IsConsumable()) {
            inventoryList[1].Add(newItem);
        } else if (newItem.IsMaterial()) {
            inventoryList[2].Add(newItem);
        }
    }
    public static void AddItem(string json) {
        AddItem(LoadItemFromJson(json));
    }
}