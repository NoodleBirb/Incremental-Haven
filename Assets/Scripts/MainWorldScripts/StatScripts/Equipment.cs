

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class Equipment : MonoBehaviour{

    private static Dictionary<string, Item> equippedItems;
    private static (GameObject, GameObject)[] slotContainersAndBodyParts;
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
        Vector2[] allPoints = new Vector2[16];
        int i = 0;
        foreach((GameObject container, GameObject bodyPart) in slotContainersAndBodyParts) {
            allPoints[i] = container.GetComponent<RectTransform>().position;
            allPoints[i+1] = Camera.main.WorldToScreenPoint(bodyPart.transform.position);
            i+=2;
        }
        GameObject.Find("Equipment Line Creator").GetComponent<UILineRenderer>().Points = allPoints;
    }

    public static Dictionary<string, Item> GetEquippedItems() {
        return equippedItems;
    }

    public static void EnableLines() {
        GameObject.Find("Equipment Line Creator").GetComponent<UILineRenderer>().enabled = true;
    }
    public static void DisableLines() {
        GameObject.Find("Equipment Line Creator").GetComponent<UILineRenderer>().enabled = false;
    }
}