using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Defective.JSON;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerStatistics : MonoBehaviour {
    public static Dictionary<string, float> totalStats;
    public static float currentHP;
    public static float currentMana;
    static bool healthCoroutineRunning;
    static bool manaCoroutineRunning;


    void Start() {
        healthCoroutineRunning = false;
        manaCoroutineRunning = false;
        UpdateStats();
        if ((int)(currentHP + 0.5) == 0) {
            currentHP = totalStats["HP"];
            currentMana = totalStats["mana"];
            GameObject.Find("Health Bar").GetComponent<Slider>().maxValue = totalStats["HP"];
            GameObject.Find("Mana Bar").GetComponent<Slider>().maxValue = totalStats["mana"];
        }
        GameObject.Find("Health Bar").GetComponent<Slider>().value = currentHP;
        GameObject.Find("Mana Bar").GetComponent<Slider>().value = currentMana;
        GameObject.Find("Health Circle Text").GetComponent<TextMeshProUGUI>().text = ((int)currentHP) + "";
        GameObject.Find("Mana Circle Text").GetComponent<TextMeshProUGUI>().text = ((int)currentMana) + "";
    }

    public static void UpdateStats() {
        float oldMaxMana = 0;
        if (totalStats != null) {
            oldMaxMana = totalStats["mana"];
        }
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
        if (GameObject.Find("Health and Mana Canvas") != null && oldMaxMana != 0) {
            currentMana = currentMana * totalStats["mana"] / oldMaxMana;
            GameObject.Find("Mana Bar").GetComponent<Slider>().maxValue = totalStats["mana"];
            GameObject.Find("Health Bar").GetComponent<Slider>().maxValue = totalStats["HP"];
            GameObject.Find("Health Bar").GetComponent<Slider>().value = currentHP;
            GameObject.Find("Mana Bar").GetComponent<Slider>().value = currentMana;
            GameObject.Find("Health Circle Text").GetComponent<TextMeshProUGUI>().text = ((int)currentHP) + "";
            GameObject.Find("Mana Circle Text").GetComponent<TextMeshProUGUI>().text = ((int)currentMana) + "";
        }
        if (GameObject.Find("Inventory Canvas") != null) {
            UpdateInventoryStats();
        }

        
    }

    void Update() {
        if (currentHP < totalStats["HP"] && !healthCoroutineRunning) {
            StartCoroutine(RegenHealth());
        }
        if (currentMana < totalStats["mana"] && !manaCoroutineRunning) {
            StartCoroutine(RegenMana());
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

    IEnumerator RegenHealth() {
        healthCoroutineRunning = true;
        Slider healthCircleSlider = GameObject.Find("Health Circle").GetComponent<Slider>();
        while (currentHP < totalStats["HP"]) {
            while (healthCircleSlider.value < 1) {
                healthCircleSlider.value += 0.01f;
                yield return new WaitForSeconds(0.1f);
            }
            currentHP += 0.04f * totalStats["HP"];
            UpdateStats();
            healthCircleSlider.value = 0;
        }
        currentHP = totalStats["HP"];
        UpdateStats();
        healthCoroutineRunning = false;
    }
    IEnumerator RegenMana() {
        manaCoroutineRunning = true;
        Slider manaCircleSlider = GameObject.Find("Mana Circle").GetComponent<Slider>();
        while (currentMana < totalStats["mana"]) {
            while (manaCircleSlider.value < 1) {
                manaCircleSlider.value += 0.01f;
                yield return new WaitForSeconds(0.1f);
            }
            currentMana += 0.1f * totalStats["mana"];
            UpdateStats();
            manaCircleSlider.value = 0;
        }
        currentMana = totalStats["mana"];
        UpdateStats();
        manaCoroutineRunning = false;
    }
    
}