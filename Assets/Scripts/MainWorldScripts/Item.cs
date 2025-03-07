

using System.Collections.Generic;

public class Item {
    private readonly string name;
    private readonly int id;
    // Stats will not matter if equippable/consumable is false
    private readonly bool equippable;
    private readonly bool consumable;
    private readonly bool material;
    private readonly Dictionary<string, float> stats;
    private readonly Dictionary<string, bool> specific_functions;
    private readonly string description;


    public Item (string name, int id, bool equippable, bool consumable, bool material, Dictionary<string, float> stats, Dictionary<string, bool> specific_functions, string description) {
        this.name = name;
        this.id = id;
        this.equippable = equippable;
        this.stats = stats;
        this.specific_functions = specific_functions;
        this.description = description;
        this.consumable = consumable;
        this.material = material;
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
    public bool IsMaterial() {
        return material;
    }
    public Dictionary<string, float> GetStats() {
        return stats;
    }
    public Dictionary<string, bool> GetSpecificFunctions() {
        return specific_functions;
    }
    public float GetStrength() {
        if (!equippable || !consumable) return 0;
        return stats["strength"];
    }
    public float GetSpeed() {
        if (!equippable || !consumable) return 0;
        return stats["speed"];
    }
    public float GetMana() {
        if (!equippable || !consumable) return 0;
        return stats["mana"];
    }
    public float GetResistance() {
        if (!equippable || !consumable) return 0;
        return stats["resistance"];
    }
    public float GetDefense() {
        if (!equippable || !consumable) return 0;
        return stats["defense"];
    }
    public float GetElementalDefense() {
        if (!equippable || !consumable) return 0;
        return stats["elemental_defense"];
    }
    public float GetElementalAffinity() {
        if (!equippable || !consumable) return 0;
        return stats["elemental_affinity"];
    }
    public float GetHealth() {
        if (!consumable) return 0;
        return stats["HP"];
    }
    public string GetDescription() {
        return description;
    }
    public bool IsConsumable() {
        return consumable;
    }





}