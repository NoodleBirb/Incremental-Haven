
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OneHandedCombat : ISkillInterface
{
    float oneHandedCombatEXP;
    int oneHandedCombatLevel;
    float nextEXPThreshold = 40;
    public float GetEXP() {
        return oneHandedCombatEXP;
    }

    public string GetName() {
        return "OneHandedCombat";
    }
    public OneHandedCombat() {
        oneHandedCombatEXP = 0f;
        oneHandedCombatLevel = 1;
        nextEXPThreshold = 40;
    }

    public Dictionary<string, float> GetStats() {
        return new () {
            ["strength"] = 0f,
            ["speed"] = 0f,
            ["mana"] = 0f,
            ["resistance"] = 0f,
            ["defence"] = 0f,
            ["elemental_defence"] = 0f,
            ["elemental_affinity"] = 0f
        };
    }

    public void IncreaseEXP(float increase) {
        oneHandedCombatEXP += increase;
        if (oneHandedCombatEXP >= nextEXPThreshold) {
            LevelUp();
        }
    }

    public int GetLevel() {
        return oneHandedCombatLevel;
    }
    void LevelUp() {
        oneHandedCombatLevel += 1;
        oneHandedCombatEXP -= nextEXPThreshold;
        nextEXPThreshold += nextEXPThreshold * (float)Math.Pow(2, 0.1);
        Debug.Log(nextEXPThreshold);
    }
    public bool IsElementalSkill() {
        return false;
    }
}