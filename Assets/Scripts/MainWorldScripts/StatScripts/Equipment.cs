

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour{

    private static Dictionary<string, Item> equippedItems;
    private (GameObject, GameObject)[] slotContainersAndBodyParts;
    void Start() {
        equippedItems ??= new() {
            ["Weapon Slot"] = null,
            ["Off Hand Slot"] = null,
            ["Necklace Slot"] = null,
            ["Glove Slot"] = null,
            ["Leggings Slot"] = null,
            ["Boots Slot"] = null,
            ["Head Piece Slot"] = null,
            ["Chestplate Slot"] = null
        };
        slotContainersAndBodyParts = new (GameObject, GameObject)[] {
            (GameObject.Find("Head Piece Slot Container"), GameObject.Find("Paper Bag")),
            (GameObject.Find("Chestplate Slot Container"), GameObject.Find("Torso")),
            (GameObject.Find("Weapon Slot Container"), GameObject.Find("Left Arm")),
            (GameObject.Find("Leggings Slot Container"), GameObject.Find("Left Leg")),
            (GameObject.Find("Necklace Slot Container"), GameObject.Find("Neck")),
            (GameObject.Find("Boots Slot Container"), GameObject.Find("Right Foot")),
            (GameObject.Find("Off Hand Slot Container"), GameObject.Find("Right Arm")),
            (GameObject.Find("Glove Slot Container"), GameObject.Find("Right Arm"))
        };
    }

    void Update() {
        if (Inventory.fullyOpen) {

            foreach((GameObject container, GameObject bodyPart) in slotContainersAndBodyParts) {

            }

            LineRenderer selectedRenderer = GameObject.Find("Head Piece Slot Container").GetComponent<LineRenderer>();

            selectedRenderer.enabled = true;

            

        } else {
        }
    }

    Vector3 WorldPositionFromUI(RectTransform uiElement) {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(uiElement, uiElement.position, Camera.main, out Vector3 worldPos);
        return worldPos;
    }

    public static void SwapEquipmentPiece(Item item, string type) {
        
        Debug.Log(type);
        Item tempItem = equippedItems[type];
        equippedItems[type] = item;
        Inventory.AddItem(tempItem);
        GameObject.Find(type).GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find(type).GetComponent<Button>().interactable = false;
        GameObject.Find(type).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/transparentbackground");
        GameObject.Find(type).GetComponent<MouseOverItem>().SetItem(null);
        MouseOverItem.ItemVanished();
        PlayerStatistics.UpdateStats();
        Inventory.LoadInventory();
    }

    public static Dictionary<string, Item> GetEquippedItems() {
        return equippedItems;
    }
}