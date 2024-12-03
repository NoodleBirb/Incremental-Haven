

using System.Collections.Generic;

public class Move {
    private readonly string name;
    private readonly int power;
    private readonly int manaCost;
    private readonly string element;
    private readonly Dictionary<string, bool> specificFunctions;
    public Move (string name, int power, int manaCost, string element, Dictionary<string, bool> specificFunctions) {
        this.name = name;
        this.power = power;
        this.manaCost = manaCost;
        this.element = element;
        this.specificFunctions = specificFunctions;
    }

    public string GetName() {
        return name;
    }
    public int GetPower() {
        return power;
    }
    public int GetManaCost() {
        return manaCost;
    }
    public string GetElement() {
        return element;
    }
    public bool GetFunction(string function) {
        return specificFunctions[function];
    }
}