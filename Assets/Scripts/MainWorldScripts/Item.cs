

using System.Collections.Generic;

public class Item {
    private readonly string name;
    private readonly int id;
    // Stats will not matter if equippable is false
    private readonly bool equippable;
    private readonly Dictionary<string, float> stats;
    private readonly Dictionary<string, bool> specific_functions;
    private readonly string description;


    public Item (string name, int id, bool equippable, Dictionary<string, float> stats, Dictionary<string, bool> specific_functions, string description) {
        this.name = name;
        this.id = id;
        this.equippable = equippable;
        this.stats = stats;
        this.specific_functions = specific_functions;
        this.description = description;
    }

    public string GetName() {
        return name;
    }
    public int GetID() {
        return id;
    }
    public bool IsEquippable() {
        return equippable;
    }
    public Dictionary<string, float> GetStats() {
        return stats;
    }
    public Dictionary<string, bool> GetSpecificFunctions() {
        return specific_functions;
    }
    public float GetStrength() {
        if (!equippable) return 0;
        return stats["strength"];
    }
    public float GetSpeed() {
        if (!equippable) return 0;
        return stats["speed"];
    }
    public float GetMana() {
        if (!equippable) return 0;
        return stats["mana"];
    }
    public float GetResistance() {
        if (!equippable) return 0;
        return stats["resistance"];
    }
    public float GetDefense() {
        if (!equippable) return 0;
        return stats["defense"];
    }
    public float GetElementalDefense() {
        if (!equippable) return 0;
        return stats["elemental_defense"];
    }
    public float GetElementalAffinity() {
        if (!equippable) return 0;
        return stats["elemental_affinity"];
    }
    public string GetDescription() {
        return description;
    }





}