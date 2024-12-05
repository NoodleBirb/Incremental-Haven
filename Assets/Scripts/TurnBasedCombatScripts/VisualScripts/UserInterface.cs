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
    Rect playerHealthRect;
    Rect enemyHealthRect;
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

        playerHealthRect = new(40f, Screen.height - 40f, rectWidth * 3 / 4, 30f);
        enemyHealthRect = new(Screen.width - 3 * rectWidth / 4 - 30, 40f, 3 * rectWidth / 4, 30f);


        FillMoveLists();
    }



    void OnGUI() {

        GUI.Box (playerHealthRect, PlayerStatistics.currentHP + "/" + PlayerStatistics.totalStats["HP"]);
        GUI.Box (enemyHealthRect, EnemyStatistics.totalCurrentHP[0] + "/" + EnemyStatistics.allEnemyStats[0]["HP"]);

        if (TurnDecider.turnOrder[0] == "player" && !InMenu()) {
            if (GUI.Button(elementalSkillRect, "Elemental Skill")) {
                Debug.Log("open elemental skill menu");
                openElementalSkill = true;
            }
            if (GUI.Button(weaponSkillRect, "Weapon Skill")) {
                Debug.Log("open weapon skill menu");
                openWeaponSkill = true;
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
                    if (PlayerStatistics.currentMana > move.GetManaCost()) {
                        Debug.Log("I used move: " + move.GetName() + " which had: " + move.GetPower() + " power~!");
                        PlayerStatistics.currentMana -= move.GetManaCost();
                        ElementalAttackEnemy(move);
                        openElementalSkill = false;
                        TurnDecider.NextTurn();
                    } else {
                        Debug.Log("Not enough mana!");
                        openElementalSkill = false;
                    }
                }
                i++;
            }
        }
        if (openWeaponSkill) {
            float rectXPos = Screen.width / 2 + 20;
            float rectYPos = Screen.height / 2;
            float rectHeight = 40f;
            float rectWidth = Screen.width / 2 - 40;
            int i = 0;
            foreach (Move move in weaponSkills) {
                if (GUI.Button(new(rectXPos, rectYPos + rectHeight * i, rectWidth, rectHeight), move.GetName())) {
                    if (PlayerStatistics.currentMana > move.GetManaCost()) {
                        Debug.Log("I used move: " + move.GetName() + " which had: " + move.GetPower() + " power~!");
                        PlayerStatistics.currentMana -= move.GetManaCost();
                        PhysicalAttackEnemy(move);
                        openWeaponSkill = false;
                        TurnDecider.NextTurn();
                    } else {
                        Debug.Log("Not enough mana!");
                        openWeaponSkill = false;
                    }
                }
                i++;
            }
        }
        if (openInventory) {

        }
        if (openSwapSkill) {

        }
        
    }

    void FillMoveLists() {
        elementalSkills = ParseMoveNames.GetMoveList(ParseMoveNames.GetMoveNames(Skills.currentElementalSkill));
        weaponSkills = ParseMoveNames.GetMoveList(ParseMoveNames.GetMoveNames(Skills.currentWeaponSkill));

    }
    bool InMenu() {
        return openElementalSkill || openInventory || openSwapSkill || openWeaponSkill;
    }

    void PhysicalAttackEnemy(Move move) {
        EnemyStatistics.totalCurrentHP[0] -= move.GetPower() * PlayerStatistics.totalStats["strength"] / EnemyStatistics.allEnemyStats[0]["defence"]; 
    }
    void ElementalAttackEnemy (Move move) {
        EnemyStatistics.totalCurrentHP[0] -= move.GetPower() * PlayerStatistics.totalStats["elemental_affinity"] / EnemyStatistics.allEnemyStats[0]["elemental_defense"]; 
    }
}
