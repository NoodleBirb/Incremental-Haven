using System;
using System.Collections;
using System.Collections.Generic;
using Defective.JSON;
using UnityEngine;

public class Malice : ISkillInterface {

    float skillEXP;
    int skillLevel;
    float nextEXPThreshold;
    List<Move> moveList;

    public Malice() {
        skillEXP = 0f;
        skillLevel = 1;
        nextEXPThreshold = 40;
        moveList = new List<Move>(4);
    }
    // Get the exp stored in the Malice object
    public float GetEXP() {
        return skillEXP;
    }
    public float GetThreshold() {
        return nextEXPThreshold;
    }
    // Increment the exp stored in the Malice object by an 'increase'
    public void IncreaseEXP(float increase) {
        skillEXP += increase;
        if (skillEXP >= nextEXPThreshold) {
            LevelUp();
        }
    }
    // Get the name of the skill
    public string GetName() {
        return "Malice";
    }
    void LevelUp() {
        skillLevel += 1;
        skillEXP -= nextEXPThreshold;
        nextEXPThreshold += nextEXPThreshold * (float)Math.Pow(2, 0.1);
        PlayerStatistics.UpdateStats();
        Debug.Log(nextEXPThreshold);
    }

    public Dictionary<string, float> GetStats() {
        Dictionary<string, float> stats = new();
        JSONObject statData = new(Resources.Load<TextAsset>("Skills/" + GetName()).text);
        int i = 0;
        foreach (string str in statData["stats"].keys) {
            stats.Add(str, statData["stats"][str].floatValue * skillLevel / 50);
            i++;
        }
        
        return stats; 
    }
    public int GetLevel() {
        return skillLevel;
    }
    public bool IsElementalSkill() {
        return true;
    }
}