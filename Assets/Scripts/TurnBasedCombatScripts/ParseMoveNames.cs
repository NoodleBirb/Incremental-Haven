

using System.Collections.Generic;
using Defective.JSON;
using UnityEngine;

public class ParseMoveNames {
    public static List<string> GetMoveNames(ISkillInterface skill) {
        JSONObject json = new(Resources.Load<TextAsset>("Skills/" + skill.GetName()).text);
        List<string> moveNames = new();
        for (int i = 1; i <= skill.GetLevel(); i++) {
            if (json["moves"]["level_" + i] != null) {
                moveNames.Add(json["moves"]["level_" + i].stringValue);
            }
        }
        return moveNames;
    }

    public static List<Move> GetMoveList(List<string> moves) {
        JSONObject json = new(Resources.Load<TextAsset>("Other Files/all_moves").text);
        List<Move> moveList = new();

        foreach (string move in moves) {
            Dictionary<string, bool> specificFunctionDict = new();
            var functions = json["moves"][move]["specific_functions"];
            foreach (string str in functions.keys) {
                specificFunctionDict[str] = json["moves"][move]["specific_functions"][str].boolValue;
            }
            moveList.Add(new(move, json["moves"][move]["power"].intValue, json["moves"][move]["mana_cost"].intValue, json["moves"][move]["element"].stringValue, specificFunctionDict));
        }
        return moveList;
    }
}