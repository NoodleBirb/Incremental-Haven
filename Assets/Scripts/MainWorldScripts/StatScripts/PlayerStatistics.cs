using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Defective.JSON;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class PlayerStatistics : MonoBehaviour {
    public static Dictionary<string, float> totalStats;
    private static Equipment equipment; 
    private static Inventory inventory;
    private static Skills skills;
    static bool playerStatsInitialized = false;
    public static float currentHP;
    public static float currentMana;

    void Start() {
        totalStats = new()
        {
            ["strength"] = 1f,
            ["speed"] = 1f,
            ["mana"] = 1f,
            ["resistance"] = 1f,
            ["defence"] = 1f,
            ["elemental_defence"] = 1f,
            ["elemental_affinity"] = 1f,
            ["HP"] = 10f
        };
        if (Inventory.isInventoryInitialized && Skills.isSkillsInitialized) {
            TryUpdateStats();
        }
        else if (!Skills.isSkillsInitialized) {
            Skills.OnSkillsInitialized += TryUpdateStats;
        }
        else if (!Inventory.isInventoryInitialized) {
            Inventory.OnInventoryInitialized += TryUpdateStats;
        }
        else {
            Skills.OnSkillsInitialized += TryUpdateStats;
            Inventory.OnInventoryInitialized += TryUpdateStats;
        }
    }
    void TryUpdateStats() {
        if (Skills.isSkillsInitialized && Inventory.isInventoryInitialized) {
            equipment = GetComponent<Equipment>();
            inventory = GetComponent<Inventory>();
            skills = GetComponent<Skills>();
            UpdateStats();
        }
    }

    public static void UpdateStats() {
        totalStats = new()
        {
            ["strength"] = 1f,
            ["speed"] = 1f,
            ["mana"] = 1f,
            ["resistance"] = 1f,
            ["defence"] = 1f,
            ["elemental_defence"] = 1f,
            ["elemental_affinity"] = 1f,
            ["HP"] = totalStats["HP"] + (Skills.playerIncrementality / 6) + 1
        };
        List<Item> equippedItems = new()
        {
            equipment.GetWeaponSlot()
        };
        foreach (Item item in equippedItems) {
            if (item != null) {
                foreach(string key in item.GetStats().Keys.ToList<string>()) {
                    totalStats[key] += item.GetStats()[key];
                }
            }
        }
        foreach (string key in skills.stats.Keys) {
            totalStats[key] += skills.stats[key];
        }
        currentHP = totalStats["HP"];
        currentMana = totalStats["mana"];
        playerStatsInitialized = true;
    }
     void OnGUI() {
        if (playerStatsInitialized && Inventory.showInventory && !inventory.shifting && !inventory.stillNotCloseEnough) {
            int topStatBoxWidth = Screen.width / 8;
            int bottomStatBoxWidth = Screen.width / 6;
            int startPos = Screen.width / 2;
            int yPos = Screen.height - 60;
            GUI.Box(new(startPos, yPos, Screen.width / 2, 60), "");
            GUI.Box(new(startPos, yPos, topStatBoxWidth, 30), "Strength: " + totalStats["strength"]);
            GUI.Box(new(startPos + topStatBoxWidth, yPos, topStatBoxWidth, 30), "Speed: " + totalStats["speed"]);
            GUI.Box(new(startPos + 2*topStatBoxWidth, yPos, topStatBoxWidth, 30), "Resistance: " + totalStats["resistance"]);
            GUI.Box(new(startPos + 3*topStatBoxWidth, yPos, topStatBoxWidth, 30), "Mana: " + totalStats["mana"]);
            yPos += 30;
            GUI.Box(new(startPos, yPos, bottomStatBoxWidth, 30), "Defence: " + totalStats["defence"]);
            GUI.Box(new(startPos + bottomStatBoxWidth, yPos, bottomStatBoxWidth, 30), "Elemental Defence: " + totalStats["elemental_defence"]);
            GUI.Box(new(startPos + 2*bottomStatBoxWidth, yPos, bottomStatBoxWidth, 30), "Elemental Affinity: " + totalStats["elemental_affinity"]);
        }
     }

}