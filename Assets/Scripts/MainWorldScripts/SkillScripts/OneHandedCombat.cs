
using System.Collections.Generic;
using UnityEditor;

public class OneHandedCombat : ISkillInterface
{
    float oneHandedCombatEXP;
    int oneHandedCombatLevel;
    public float GetEXP() {
        return oneHandedCombatEXP;
    }

    public string GetName() {
        return "OneHandedCombat";
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
    }

    public int GetLevel() {
        return oneHandedCombatLevel;
    }

}