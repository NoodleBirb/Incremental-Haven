using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Defective.JSON;

public class PlayerStatistics : MonoBehaviour {
    private Dictionary<string, float> stats;
    private Equipment equipment; 
    private Inventory inventory;
    private Skills skills;
    bool playerStatsInitialized = false;

    void Start() {
        stats = new()
        {
            ["strength"] = 1f,
            ["speed"] = 1f,
            ["mana"] = 1f,
            ["resistance"] = 1f,
            ["defence"] = 1f,
            ["elemental_defence"] = 1f,
            ["elemental_affinity"] = 1f
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

    public void UpdateStats() {
        stats = new()
        {
            ["strength"] = 1f,
            ["speed"] = 1f,
            ["mana"] = 1f,
            ["resistance"] = 1f,
            ["defence"] = 1f,
            ["elemental_defence"] = 1f,
            ["elemental_affinity"] = 1f
        };
        List<Item> equippedItems = new()
        {
            equipment.GetWeaponSlot()
        };
        foreach (Item item in equippedItems) {
            if (item != null) {
                foreach(string key in stats.Keys.ToList<string>()) {
                    stats[key] += item.GetStats()[key];
                }
            }
        }
        foreach (string key in skills.stats.Keys) {
            stats[key] += skills.stats[key];
        }
        PlayerPrefs.SetFloat("strength_player", stats["strength"]);
        PlayerPrefs.SetFloat("speed_player", stats["speed"]);
        PlayerPrefs.SetFloat("mana_player", stats["mana"]);
        PlayerPrefs.SetFloat("resistance_player", stats["resistance"]);
        PlayerPrefs.SetFloat("defence_player", stats["defence"]);
        PlayerPrefs.SetFloat("elemental_defence_player", stats["elemental_defence"]);
        PlayerPrefs.SetFloat("elemental_affinity_player", stats["elemental_affinity"]);
        playerStatsInitialized = true;
    }
     void OnGUI() {
        if (playerStatsInitialized && Inventory.showInventory && !inventory.shifting && !inventory.stillNotCloseEnough) {
            int topStatBoxWidth = Screen.width / 8;
            int bottomStatBoxWidth = Screen.width / 6;
            int startPos = Screen.width / 2;
            int yPos = Screen.height - 60;
            GUI.Box(new(startPos, yPos, Screen.width / 2, 60), "");
            GUI.Box(new(startPos, yPos, topStatBoxWidth, 30), "Strength: " + stats["strength"]);
            GUI.Box(new(startPos + topStatBoxWidth, yPos, topStatBoxWidth, 30), "Speed: " + stats["speed"]);
            GUI.Box(new(startPos + 2*topStatBoxWidth, yPos, topStatBoxWidth, 30), "Resistance: " + stats["resistance"]);
            GUI.Box(new(startPos + 3*topStatBoxWidth, yPos, topStatBoxWidth, 30), "Mana: " + stats["mana"]);
            yPos += 30;
            GUI.Box(new(startPos, yPos, bottomStatBoxWidth, 30), "Defence: " + stats["defence"]);
            GUI.Box(new(startPos + bottomStatBoxWidth, yPos, bottomStatBoxWidth, 30), "Elemental Defence: " + stats["elemental_defence"]);
            GUI.Box(new(startPos + 2*bottomStatBoxWidth, yPos, bottomStatBoxWidth, 30), "Elemental Affinity: " + stats["elemental_affinity"]);
        }
     }
}