

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Consumables : MonoBehaviour{

    private static List<Item> currentConsumables;
    void Awake() {
        currentConsumables ??= new() {

        };
    }

    void Update() {
        // Eventually update with code to slowly turn off the active consumable
    }

    public static List<Item> GetCurrentConsumables() {
        return currentConsumables;
    }
    public static void UseConsumable(Item consumable) {
        Debug.Log(currentConsumables);
        currentConsumables.Add(consumable);
        Inventory.inventoryList[1].Remove(consumable);
        Inventory.LoadInventory();
        PlayerStatistics.UpdateStats();
    }
}