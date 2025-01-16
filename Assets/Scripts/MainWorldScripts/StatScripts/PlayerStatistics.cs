using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Defective.JSON;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using TMPro;

public class PlayerStatistics : MonoBehaviour {
    public static Dictionary<string, float> totalStats;
    private static Equipment equipment; 
    private static Inventory inventory;
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
            ["defense"] = 1f,
            ["elemental_defense"] = 1f,
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
            ["defense"] = 1f,
            ["elemental_defense"] = 1f,
            ["elemental_affinity"] = 1f,
            ["HP"] = 10f + (Skills.playerIncrementality * 6) // Update this formula whenever all the skills are added.
        };
        List<Item> equippedItems = new()
        {
            Equipment.GetWeaponSlot()
        };
        foreach (Item item in equippedItems) {
            if (item != null) {
                foreach(string key in item.GetStats().Keys.ToList<string>()) {
                    totalStats[key] += item.GetStats()[key];
                }
            }
        }
        Skills.UpdateElementalSkillStats();
        foreach (string key in Skills.stats.Keys) {
            totalStats[key] += Skills.stats[key];
        }
        currentHP = totalStats["HP"];
        currentMana = totalStats["mana"];
        playerStatsInitialized = true;
    }

    public static void UpdateInventoryStats() {
        GameObject.Find("HP").GetComponentInChildren<TextMeshProUGUI>().text = "" + totalStats["HP"];
        GameObject.Find("Strength").GetComponentInChildren<TextMeshProUGUI>().text = "" + totalStats["strength"];
        GameObject.Find("Speed").GetComponentInChildren<TextMeshProUGUI>().text = "" + totalStats["speed"];
        GameObject.Find("Resistance").GetComponentInChildren<TextMeshProUGUI>().text = "" + totalStats["resistance"];
        GameObject.Find("Mana").GetComponentInChildren<TextMeshProUGUI>().text = "" + totalStats["mana"];
        GameObject.Find("Defense").GetComponentInChildren<TextMeshProUGUI>().text = "" + totalStats["defense"];
        GameObject.Find("Elemental Defense").GetComponentInChildren<TextMeshProUGUI>().text = "" + totalStats["elemental_defense"];
        GameObject.Find("Elemental Affinity").GetComponentInChildren<TextMeshProUGUI>().text = "" + totalStats["elemental_affinity"];
    }
}