
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LightRanged : ISkillInterface
{
    float skillEXP;
    int skillLevel;
    float nextEXPThreshold = 40;
    public float GetEXP() {
        return skillEXP;
    }

    public string GetName() {
        return "LightRanged";
    }
    public float GetThreshold() {
        return nextEXPThreshold;
    }
    public LightRanged() {
        skillEXP = 0f;
        skillLevel = 1;
        nextEXPThreshold = 40;
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

    public void IncreaseEXP(float increase) {
        skillEXP += increase;
        if (skillEXP >= nextEXPThreshold) {
            LevelUp();
        }
    }

    public int GetLevel() {
        return skillLevel;
    }
    void LevelUp() {
        skillLevel += 1;
        skillEXP -= nextEXPThreshold;
        nextEXPThreshold += nextEXPThreshold * (float)Math.Pow(2, 0.1);
        Debug.Log(nextEXPThreshold);
    }
    public bool IsElementalSkill() {
        return false;
    }
}