using System;
using System.Collections;
using System.Collections.Generic;
using Defective.JSON;
using UnityEngine;

public class Fishing : ISkillInterface {

    float fishingEXP;
    int fishingLevel;
    float nextEXPThreshold;


    public Fishing() {
        fishingEXP = 0f;
        fishingLevel = 1;
        nextEXPThreshold = 40;
    }
    // Get the exp stored in the Woodcutting object
    public float GetEXP() {
        return fishingEXP;
    }
    public float GetThreshold() {
        return nextEXPThreshold;
    }
    // Increment the exp stored in the Woodcutting object by an 'increase'
    public void IncreaseEXP(float increase) {
        fishingEXP += increase;
        if (fishingEXP >= nextEXPThreshold) {
            LevelUp();
        }
    }
    // Get the name of the skill
    public string GetName() {
        return "Fishing";
    }
    void LevelUp() {
        fishingLevel += 1;
        fishingEXP -= nextEXPThreshold;
        nextEXPThreshold += nextEXPThreshold * (float)Math.Pow(2, 0.1);
        PlayerStatistics.UpdateStats();
        Debug.Log(nextEXPThreshold);
    }

    public Dictionary<string, float> GetStats() {
        Dictionary<string, float> stats = new();
        JSONObject statData = new(Resources.Load<TextAsset>("Skills/Fishing").text);
        int i = 0;
        foreach (string str in statData["stats"].keys) {
            stats.Add(str, statData["stats"][str][fishingLevel - 1].floatValue);
            i++;
        }
        
        return stats; 
    }
    public int GetLevel() {
        return fishingLevel;
    }
    public bool IsElementalSkill() {
        return true;
    }
}