using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Defective.JSON;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using TMPro;

public class PlayerStatistics : MonoBehaviour {
    public static Dictionary<string, float> totalStats;
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
        UpdateStats();
        if ((int)(currentHP + 0.5) == 0) {
            currentHP = totalStats["HP"];
            currentMana = totalStats["mana"];
        }
    }

    public static void UpdateStats() {
        Debug.Log(currentHP);
        float oldMaxHP = totalStats["HP"];
        float oldMaxMana = totalStats["mana"];
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
        foreach (Item item in Equipment.GetEquippedItems().Values) {
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
        currentHP = totalStats["HP"] * currentHP / oldMaxHP;
        currentMana = totalStats["mana"] * currentMana / oldMaxMana;
        if (GameObject.Find("Inventory Canvas") != null) {
            UpdateInventoryStats();
        }
    }

    public static void UpdateInventoryStats() {
        GameObject.Find("HP").transform.Find("Data").GetComponent<TextMeshProUGUI>().text = "" + totalStats["HP"];
        GameObject.Find("Strength").transform.Find("Data").GetComponent<TextMeshProUGUI>().text = "" + totalStats["strength"];
        GameObject.Find("Speed").transform.Find("Data").GetComponent<TextMeshProUGUI>().text = "" + totalStats["speed"];
        GameObject.Find("Resistance").transform.Find("Data").GetComponent<TextMeshProUGUI>().text = "" + totalStats["resistance"];
        GameObject.Find("Mana").transform.Find("Data").GetComponent<TextMeshProUGUI>().text = "" + totalStats["mana"];
        GameObject.Find("Defense").transform.Find("Data").GetComponent<TextMeshProUGUI>().text = "" + totalStats["defense"];
        GameObject.Find("Elemental Defense").transform.Find("Data").GetComponent<TextMeshProUGUI>().text = "" + totalStats["elemental_defense"];
        GameObject.Find("Elemental Affinity").transform.Find("Data").GetComponent<TextMeshProUGUI>().text = "" + totalStats["elemental_affinity"];
    }
}