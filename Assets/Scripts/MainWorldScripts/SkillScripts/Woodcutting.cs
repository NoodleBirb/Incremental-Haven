using System;
using System.Collections;
using System.Collections.Generic;
using Defective.JSON;
using UnityEngine;

public class Woodcutting : ISkillInterface {

    float woodcuttingEXP;
    int woodCuttingLevel;
    float nextEXPThreshold;


    public Woodcutting() {
        woodcuttingEXP = 0f;
        woodCuttingLevel = 1;
        nextEXPThreshold = 40;
    }
    // Get the exp stored in the Woodcutting object
    public float GetEXP() {
        return woodcuttingEXP;
    }
    // Increment the exp stored in the Woodcutting object by an 'increase'
    public void IncreaseEXP(float increase) {
        woodcuttingEXP += increase;
        if (woodcuttingEXP >= nextEXPThreshold) {
            LevelUp();
        }
    }
    // Get the name of the skill
    public string GetName() {
        return "Woodcutting";
    }
    void LevelUp() {
        woodCuttingLevel += 1;
        woodcuttingEXP -= nextEXPThreshold;
        nextEXPThreshold += nextEXPThreshold * (float)Math.Pow(2, 0.1);
        PlayerStatistics.UpdateStats();
        Debug.Log(nextEXPThreshold);
    }

    public Dictionary<string, float> GetStats() {
        Dictionary<string, float> stats = new();
        JSONObject statData = new(Resources.Load<TextAsset>("Skills/Woodcutting").text);
        int i = 0;
        foreach (string str in statData["stats"].keys) {
            stats.Add(str, statData["stats"][str][woodCuttingLevel - 1].floatValue);
            i++;
        }
        
        return stats; 
    }
    public int GetLevel() {
        return woodCuttingLevel;
    }
    public bool IsElementalSkill() {
        return true;
    }
}