

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Consumables : MonoBehaviour{

    private static Dictionary<Item, bool> currentConsumables;
    void Awake() {
        currentConsumables ??= new() {

        };
    }

    void Update() {
        // Eventually update with code to slowly turn off the active consumable
    }

    public static List<Item> GetCurrentConsumables() {
        return currentConsumables.Keys.ToList<Item>();
    }
    public static Dictionary<Item, bool> GetConsumableDict() {
        return currentConsumables;
    }
    public static void UseConsumable(Item consumable) {
        currentConsumables[consumable] = false;
        Inventory.inventoryList[1].Remove(consumable);
        if (!Inventory.inventoryList[1].Contains(consumable)) {
            MouseOverItem.ItemVanished();
        }
        Inventory.LoadInventory();
        PlayerStatistics.UpdateStats();
    }
    public static void UseCombatConsumable(Item consumable) {
        currentConsumables[consumable] = false;
        Inventory.inventoryList[1].Remove(consumable);
        PlayerStatistics.UpdateStats();
        GameObject.Find("Player HP Slider").GetComponent<Slider>().value = PlayerStatistics.currentHP;
        if (!Inventory.inventoryList[1].Contains(consumable)) {
            MouseOverItem.ItemVanished();
        }
        Inventory.LoadCombatInventory();
        GameObject.Find("Inventory Canvas").GetComponent<Canvas>().enabled = false;
        TurnDecider.NextTurn();
    }
}