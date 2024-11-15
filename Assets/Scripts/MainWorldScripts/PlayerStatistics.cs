using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Defective.JSON;

public class PlayerStatistics : MonoBehaviour {
    private Dictionary<string, float> stats;
    private Equipment equipment; 
    private Inventory inventory;

    void Start() {
        equipment = GetComponent<Equipment>();
        inventory = GetComponent<Inventory>();
        if (Inventory.isInventoryInitialized) {
            UpdateStats();
        }
        else {
            Inventory.OnInventoryInitialized += UpdateStats;
        }
        PlayerPrefs.SetFloat("strength", stats["strength"]);
        PlayerPrefs.SetFloat("speed", stats["speed"]);
        PlayerPrefs.SetFloat("mana", stats["mana"]);
        PlayerPrefs.SetFloat("resistance", stats["resistance"]);
        PlayerPrefs.SetFloat("defence", stats["defence"]);
        PlayerPrefs.SetFloat("elemental_defence", stats["elemental_defence"]);
        PlayerPrefs.SetFloat("elemental_affinity", stats["elemental_affinity"]);
    }

    public void UpdateStats() {
        stats = new()
        {
            ["strength"] = 0f,
            ["speed"] = 0f,
            ["mana"] = 0f,
            ["resistance"] = 0f,
            ["defence"] = 0f,
            ["elemental_defence"] = 0f,
            ["elemental_affinity"] = 0f
        };
        List<Item> equippedItems = new()
        {
            equipment.GetWeaponSlot()
        };
        foreach (Item item in equippedItems) {
            if (item != null) {
                foreach(string key in stats.Keys.ToList<string>()) {
                    Debug.Log(key);
                    stats[key] += item.GetStats()[key];
                }
            }
        }
        PlayerPrefs.SetFloat("strength_player", stats["strength"]);
        PlayerPrefs.SetFloat("speed_player", stats["speed"]);
        PlayerPrefs.SetFloat("mana_player", stats["mana"]);
        PlayerPrefs.SetFloat("resistance_player", stats["resistance"]);
        PlayerPrefs.SetFloat("defence_player", stats["defence"]);
        PlayerPrefs.SetFloat("elemental_defence_player", stats["elemental_defence"]);
        PlayerPrefs.SetFloat("elemental_affinity_player", stats["elemental_affinity"]);
    }
     void OnGUI() {
        if (inventory.showInventory && !inventory.shifting && !inventory.stillNotCloseEnough) {
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