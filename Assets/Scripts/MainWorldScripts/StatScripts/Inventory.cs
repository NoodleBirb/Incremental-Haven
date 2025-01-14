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

    void OnGUI() {
        if (!showInventory && GUI.Button(openInventoryRect, "Inventory")) {
            showInventory = true;
            stillNotCloseEnough = true;
            PlayerMovement.openGUI = false;
            startTime = Time.time;
            intitialPos = mainCamera.transform.position;
            player.transform.LookAt(new Vector3(mainCamera.transform.position.x, 0, mainCamera.transform.position.z));
        }
        if (showInventory && !shifting && !stillNotCloseEnough) {
            int buttonWidth = (Screen.width / 2 - 60) / 3;
            int startX = Screen.width / 2;

            // okay i give up on understanding why this won't make a clean placement of the buttons. It's not my problem anymore. 
            if (GUI.Button(new(startX, 0, buttonWidth, 30), "Tab 1")) {
                showInventory = false;
            }
            if (GUI.Button(new(startX + buttonWidth, 0, buttonWidth, 30), "Tab 2")) {
                showInventory = false;
            }
            if (GUI.Button(new(startX + 2*buttonWidth, 0, buttonWidth, 30), "Tab 3")) {
                showInventory = false;
            }
            if ((startX - 60) % 3 == 0 && GUI.Button(new(Screen.width - 60, 0, 60, 30), "Close")) {
                showInventory = false;
            }
            else if ((startX - 60) % 3 == 1 && GUI.Button(new(Screen.width - 62, 0, 62, 30), "Close")) {
                showInventory = false;
            }
            else if ((startX - 60) % 3 == 2 && GUI.Button(new(Screen.width - 63, 0, 63, 30), "Close")) {
                showInventory = false;
            }

            scrollPosition = GUI.BeginScrollView(new Rect(Screen.width / 2 , 30, Screen.width / 2, Screen.height- 90), scrollPosition, new Rect(0, 0, Screen.width / 2, Screen.height- 50));
            GUI.Box(new Rect(0, 0, Screen.width / 2, Screen.height * 2), "");
            
            int itemBoxWidth = Screen.width / 8;
            int itemBoxYPos = 0;
            int itemBoxXPos = 0;

            // List the items. Coordinates begin in the corner of the ScrollView.
            for (int i = 0; i < inventoryList.Count; i++) {
                if (i != 0 && i % 4 == 0) {
                    itemBoxYPos += 30;
                    itemBoxXPos = 0;
                }
                if (inventoryList[i].IsEquippable() && GUI.Button(new(itemBoxXPos, itemBoxYPos, itemBoxWidth, 30), inventoryList[i].GetName())) {
                    Item previouslyEquippedItem = Equipment.SwapWeapon(inventoryList[i]);
                    if (previouslyEquippedItem != null) {
                        inventoryList.Add(previouslyEquippedItem);
                    }
                    inventoryList.RemoveAt(i);
                    PlayerStatistics.UpdateStats();
                } else if (!inventoryList[i].IsEquippable()){
                    GUI.Box(new Rect(new(itemBoxXPos, itemBoxYPos, itemBoxWidth, 30)), inventoryList[i].GetName());
                }
                itemBoxXPos += itemBoxWidth;
            }


            // End the scroll view that we began above.
            GUI.EndScrollView();
        }
    }
    void Update() {
        if (showInventory && stillNotCloseEnough) {
            StartCoroutine(ZoomIntoPlayer());
        }
        if (showInventory && shifting) {
            StartCoroutine(ShiftCameraToRight());
        }
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