
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool showInventory;
    public Dictionary<string, SkillInterface> inventory = new();
    // The position on of the scrolling viewport
    public Vector2 scrollPosition = Vector2.zero;
    public Rect windowRect = new(Screen.width / 2, Screen.height / 2, 200, 100);
    public Rect openInventoryRect;
    bool stillNotCloseEnough;
    bool shifting;
    GameObject player;
    GameObject mainCamera;
    void Start()
    {
        shifting = false;
        player = GameObject.Find("player model");
        mainCamera = GameObject.Find("Main Camera");
        stillNotCloseEnough = false;
        showInventory = false;
        openInventoryRect = new(Screen.width - 200, Screen.height - 50, 100, 50);
    }

    void OnGUI() {
        if (!showInventory && GUI.Button(openInventoryRect, "Inventory")) {
            showInventory = true;
            stillNotCloseEnough = true;
        }
        if (showInventory && !shifting && !stillNotCloseEnough) {
            

            scrollPosition = GUI.BeginScrollView(new Rect(Screen.width / 2 , 20, Screen.width / 2, Screen.height- 20), scrollPosition, new Rect(0, 0, Screen.width / 2, Screen.height- 20));
            if (GUI.Button(new(180, 20, 20, 20), "Close")) {
                showInventory = false;
            }
            // List the items. Coordinates begin in the corner of the ScrollView.
            GUI.Box(new Rect(0, 20, 200, 50),  "this is an item!");

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
        if(Vector2.Distance(new(mainCamera.transform.position.x, mainCamera.transform.position.z), new(player.transform.position.x, player.transform.position.z)) <= 2f) {
            if (mainCamera.transform.position.y >= 1.5) {
                mainCamera.transform.position = new (mainCamera.transform.position.x, mainCamera.transform.position.y - 5f * Time.deltaTime, mainCamera.transform.position.z);
                mainCamera.transform.LookAt(player.transform);
            } else {
                stillNotCloseEnough = false;
                shifting = true;
            }
        }
        else {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, new(player.transform.position.x, 1.5f, player.transform.position.z),  5f * Time.deltaTime);
            mainCamera.transform.LookAt(player.transform);
            yield return new WaitForSeconds(.1f);
        }
    }
    IEnumerator ShiftCameraToRight() {
        for (int i = 0; i < 70 /*arbitrary value i randomly set*/; i++) {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, new(mainCamera.transform.position.x + mainCamera.transform.right.x, mainCamera.transform.position.y, mainCamera.transform.position.z + mainCamera.transform.right.z), .1f * Time.deltaTime);
            yield return null;
        }
        shifting = false;
    }
}
