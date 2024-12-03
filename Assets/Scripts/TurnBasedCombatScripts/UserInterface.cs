using System.Collections;
using System.Collections.Generic;
using Defective.JSON;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterface : MonoBehaviour
{
    Rect elementalSkillRect;
    Rect weaponSkillRect;
    Rect inventoryRect;
    Rect swapSkillRect;
    Rect runRect;

    bool openElementalSkill;
    bool openWeaponSkill;
    bool openInventory;
    bool openSwapSkill;

    List<Move> elementalSkills;
    List<Move> weaponSkills;
    // Start is called before the first frame update
    void Start()
    {
        float rectXPos = Screen.width / 2 + 20;
        float rectYPos = Screen.height / 2;
        float rectHeight = 40f;
        float rectWidth = Screen.width / 2 - 40;
        elementalSkillRect = new(rectXPos, rectYPos, rectWidth, rectHeight);
        weaponSkillRect = new(rectXPos, rectYPos + 50f, rectWidth, rectHeight);
        inventoryRect = new(rectXPos, rectYPos + 2 * 50f, rectWidth, rectHeight);
        swapSkillRect = new(rectXPos, rectYPos + 3 * 50f, rectWidth, rectHeight);
        runRect = new(rectXPos, rectYPos + 4 * 50f, rectWidth, rectHeight);
        FillMoveLists();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI() {
        if (TurnDecider.turnOrder[0] == "player" && !InMenu()) {
            if (GUI.Button(elementalSkillRect, "Elemental Skill")) {
                Debug.Log("open elemental skill menu");
                openElementalSkill = true;
            }
            if (GUI.Button(weaponSkillRect, "open weapon skill menu")) {
                Debug.Log("open weapon skill menu");
            }
            if (GUI.Button(inventoryRect, "Inventory")) {
                Debug.Log("open inventory");
            }
            if (GUI.Button(swapSkillRect, "Change Skill")) {
                Debug.Log("Open change skill menu");
            }
            if (GUI.Button(runRect, "Run")) {
                SceneManager.LoadScene("firstarea");
                Debug.Log("Run away!");
            }
        }
        if (openElementalSkill) {
            float rectXPos = Screen.width / 2 + 20;
            float rectYPos = Screen.height / 2;
            float rectHeight = 40f;
            float rectWidth = Screen.width / 2 - 40;
            int i = 0;
            foreach (Move move in elementalSkills) {
                if (GUI.Button(new(rectXPos, rectYPos + rectHeight * i, rectWidth, rectHeight), move.GetName())) {
                    Debug.Log("I used move: " + move.GetName() + " which had: " + move.GetPower() + " power~!");
                }
                i++;
            }
        }
        if (openWeaponSkill) {
            
        }
        if (openInventory) {

        }
        if (openSwapSkill) {

        }
        
    }

    void FillMoveLists() {
        elementalSkills = GetMoveList(GetMoveNames(Skills.currentElementalSkill));
        weaponSkills = GetMoveList(GetMoveNames(Skills.currentWeaponSkill));

    }

    List<string> GetMoveNames(ISkillInterface skill) {
        JSONObject json = new(Resources.Load<TextAsset>("Skills/" + skill.GetName()).text);
        List<string> moveNames = new();
        for (int i = 1; i <= skill.GetLevel(); i++) {
            if (json["moves"]["level_" + i] != null) {
                moveNames.Add(json["moves"]["level_" + i].stringValue);
            }
        }
        return moveNames;
    }

    List<Move> GetMoveList(List<string> moves) {
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
    bool InMenu() {
        return openElementalSkill || openInventory || openSwapSkill || openWeaponSkill;
    }
}
