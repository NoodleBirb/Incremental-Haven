using System;
using System.Collections;
using System.Collections.Generic;
using Defective.JSON;
using UnityEngine;

public class Magic : ISkillInterface {

    float skillEXP;
    int skillLevel;
    float nextEXPThreshold;
    List<Move> moveList;

    public Magic() {
        skillEXP = 0f;
        skillLevel = 1;
        nextEXPThreshold = 40;
        moveList = new List<Move>(4);
    }
    // Get the exp stored in the Magic object
    public float GetEXP() {
        return skillEXP;
    }
    public float GetThreshold() {
        return nextEXPThreshold;
    }
    // Increment the exp stored in the Magic object by an 'increase'
    public void IncreaseEXP(float increase) {
        skillEXP += increase;
        if (skillEXP >= nextEXPThreshold) {
            LevelUp();
        }
    }
    // Get the name of the skill
    public string GetName() {
        return "Magic";
    }
    void LevelUp() {
        skillLevel += 1;
        skillEXP -= nextEXPThreshold;
        nextEXPThreshold += nextEXPThreshold * (float)Math.Pow(2, 0.1);
        PlayerStatistics.UpdateStats();
        Debug.Log(nextEXPThreshold);
    }

    public Dictionary<string, float> GetStats() {
        return new () {
            ["strength"] = 0f,
            ["speed"] = 0f,
            ["mana"] = 0f,
            ["resistance"] = 0f,
            ["defense"] = 0f,
            ["elemental_defense"] = 0f,
            ["elemental_affinity"] = 0f
        };
    }
    public int GetLevel() {
        return skillLevel;
    }
    public bool IsElementalSkill() {
        return false;
    }
}