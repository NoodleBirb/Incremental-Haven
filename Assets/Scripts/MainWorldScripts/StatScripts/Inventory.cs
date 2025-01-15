using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Defective.JSON;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    public static bool showInventory = false;
    // The position on of the scrolling viewport
    public Vector2 scrollPosition = Vector2.zero;
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

        if ((inventoryList == null || inventoryList.Count == 0) && Equipment.GetWeaponSlot() == null) {
            TextAsset stoneAxe = Resources.Load<TextAsset>("Items/stone_axe");
            Item item = LoadItemFromJson(stoneAxe.text);
            inventoryList = new() // eventually will be initialized with stuff from the saving system.
            {
                // Placeholder for testing
                item
            }; 
        }
        isInventoryInitialized = true;
        OnInventoryInitialized?.Invoke();
    }

    void Update() {
        if (showInventory && stillNotCloseEnough) {
            StartCoroutine(ZoomIntoPlayer());
        }
        if (showInventory && shifting) {
            StartCoroutine(ShiftCameraToRight());
        }
    }

    public void OpenInventory() {
        showInventory = true;
        stillNotCloseEnough = true;
        PlayerMovement.openGUI = false;
        startTime = Time.time;
        intitialPos = mainCamera.transform.position;
        GameObject.Find("World Canvas").GetComponent<Canvas>().enabled = false;
        player.transform.LookAt(new Vector3(mainCamera.transform.position.x, 0, mainCamera.transform.position.z));

        // List the items. Coordinates begin in the corner of the ScrollView.
        
    }
    public void CloseInventory() {
        showInventory = false;
        GameObject.Find("World Canvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("Inventory Canvas").GetComponent<Canvas>().enabled = false;
    }

    IEnumerator ZoomIntoPlayer() {

        if(Vector2.Distance(new(mainCamera.transform.position.x, mainCamera.transform.position.z), new(player.transform.position.x, player.transform.position.z)) <= 2.4f) {
            if (mainCamera.transform.position.y >= 1.5) {
                mainCamera.transform.position = new (mainCamera.transform.position.x, mainCamera.transform.position.y - 5f * Time.deltaTime, mainCamera.transform.position.z);
                mainCamera.transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z));
            } else {
                stillNotCloseEnough = false;
                shifting = true;
            }
        }
        else {
            mainCamera.transform.position = Vector3.Lerp(intitialPos, new(player.transform.position.x, 1.5f, player.transform.position.z), Time.time - startTime);
            mainCamera.transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z));
            yield return new WaitForSeconds(.1f);
        }
    }
    IEnumerator ShiftCameraToRight() {
        Vector2 camPos = new(mainCamera.transform.position.x, mainCamera.transform.position.z);
        Vector2 playerPos = new(player.transform.position.x, player.transform.position.z);
        Vector3 rightConversion = new(mainCamera.transform.position.x + mainCamera.transform.right.x, mainCamera.transform.position.y, mainCamera.transform.position.z + mainCamera.transform.right.z);
        
        while (Vector2.Distance(camPos, playerPos) <= 2.7f) {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, rightConversion, .05f * Time.deltaTime);
            camPos = new(mainCamera.transform.position.x, mainCamera.transform.position.z);
            rightConversion = new(mainCamera.transform.position.x + mainCamera.transform.right.x, mainCamera.transform.position.y, mainCamera.transform.position.z + mainCamera.transform.right.z);
            yield return null;
        }
        shifting = false;
        GameObject.Find("Inventory Canvas").GetComponent<Canvas>().enabled = true;
    }




    private static Item LoadItemFromJson(string json)
    {
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

        return new Item(obj["name"].stringValue, obj["id"].intValue, obj["equippable"].boolValue, statsDict, specificFunctionDict);
    }

    public static void AddItem(Item newItem) {
        inventoryList.Add(newItem);
    }
    public static void AddItem(string json) {
        inventoryList.Add(LoadItemFromJson(json));
    }


}