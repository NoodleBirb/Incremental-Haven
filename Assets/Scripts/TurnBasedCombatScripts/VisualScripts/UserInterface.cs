using System.Collections;
using System.Collections.Generic;
using Defective.JSON;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    Rect inventoryRect;
    Rect swapSkillRect;
    Rect runRect;
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
        inventoryRect = new(rectXPos, rectYPos + 2 * 50f, rectWidth, rectHeight);
        swapSkillRect = new(rectXPos, rectYPos + 3 * 50f, rectWidth, rectHeight);
        runRect = new(rectXPos, rectYPos + 4 * 50f, rectWidth, rectHeight);

        GameObject.Find("Elemental Skill Button").GetComponent<Button>().onClick.AddListener(() => OpenElementalSkill());
        GameObject.Find("Weapon Skill Button").GetComponent<Button>().onClick.AddListener(() => OpenWeaponSkill());

        FillMoveLists();
    }

    void OpenWeaponSkill() {
        int i = 0;
        GameObject.Find("Player Choices Canvas").GetComponent<Canvas>().enabled = false;
        GameObject weaponSkillCanvas = GameObject.Find("Weapon Skill Canvas");
        foreach (Move move in weaponSkills) {
            GameObject button = GameObject.Instantiate(Resources.Load<GameObject>("UI/Use Skill Button"));
            button.GetComponent<Button>().onClick.AddListener(() => UseWeaponSkill(move));
            button.GetComponent<Button>().onClick.AddListener(() => weaponSkillCanvas.GetComponent<Canvas>().enabled = false);
            
            button.GetComponentInChildren<TextMeshProUGUI>().text = move.GetName();
            button.transform.SetParent(weaponSkillCanvas.transform);
            button.GetComponent<RectTransform>().anchoredPosition = GameObject.Find("Elemental Skill Button").GetComponent<RectTransform>().anchoredPosition + new Vector2(0, (button.GetComponent<RectTransform>().rect.height + 10) * i);
            
            i++;
        }
        weaponSkillCanvas.GetComponent<Canvas>().enabled = true;
    }
    void UseWeaponSkill(Move move) {
        if (PlayerStatistics.currentMana >= move.GetManaCost()) {
            Debug.Log("I used move: " + move.GetName() + " which had: " + move.GetPower() + " power~!");
            PlayerStatistics.currentMana -= move.GetManaCost();
            PhysicalAttackEnemy(move);

            TurnDecider.NextTurn();
        } else {
            Debug.Log("Not enough mana!");
        }

    }

    void OpenElementalSkill() {
        int i = 0;
        GameObject.Find("Player Choices Canvas").GetComponent<Canvas>().enabled = false;
        GameObject elementalSkillCanvas = GameObject.Find("Elemental Skill Canvas");
        foreach (Move move in elementalSkills) {
            GameObject button = GameObject.Instantiate(Resources.Load<GameObject>("UI/Use Skill Button"));
            button.GetComponent<Button>().onClick.AddListener(() => UseElementalSkill(move));
            button.GetComponent<Button>().onClick.AddListener(() => elementalSkillCanvas.GetComponent<Canvas>().enabled = false);
            
            button.GetComponentInChildren<TextMeshProUGUI>().text = move.GetName();
            button.transform.SetParent(elementalSkillCanvas.transform);
            button.GetComponent<RectTransform>().anchoredPosition = GameObject.Find("Elemental Skill Button").GetComponent<RectTransform>().anchoredPosition + new Vector2(0, (button.GetComponent<RectTransform>().rect.height + 10) * i);
            
            i++;
        }
        elementalSkillCanvas.GetComponent<Canvas>().enabled = true;
    }
    void UseElementalSkill(Move move) {
        if (PlayerStatistics.currentMana >= move.GetManaCost()) {
            Debug.Log("I used move: " + move.GetName() + " which had: " + move.GetPower() + " power~!");
            PlayerStatistics.currentMana -= move.GetManaCost();
            ElementalAttackEnemy(move);

            TurnDecider.NextTurn();
        } else {
            Debug.Log("Not enough mana!");
        }

    }

    void OnGUI() {

        if (TurnDecider.turnOrder[0] == "player") {
            if (GUI.Button(inventoryRect, "Inventory")) {
                Debug.Log("open inventory");
            }
            if (GUI.Button(swapSkillRect, "Change Skill")) {
                Debug.Log("Open change skill menu");
                openSwapSkill = true;
            }
            if (GUI.Button(runRect, "Run")) {
                SceneManager.LoadScene("firstarea");
                Debug.Log("Run away!");
            }
        }
        if (openSwapSkill) {
            float rectXPos = Screen.width / 2 + 20;
            float rectYPos = Screen.height / 2;
            float rectHeight = 40f;
            float rectWidth = Screen.width / 2 - 40;
            int i = 0;
            foreach (ISkillInterface skill in Skills.skillList.Values) {
                if (skill.IsElementalSkill() && GUI.Button(new(rectXPos, rectYPos + rectHeight * i, rectWidth, rectHeight), skill.GetName())) {
                    Skills.currentElementalSkill = skill;
                    SetNewElementalSkills();
                    PlayerStatistics.UpdateStats();
                    openSwapSkill = false;
                    TurnDecider.NextTurn();
                }
                i++;
            }
        }
        
    }

    void FillMoveLists() {
        elementalSkills = ParseMoveNames.GetMoveList(ParseMoveNames.GetMoveNames(Skills.currentElementalSkill));
        weaponSkills = ParseMoveNames.GetMoveList(ParseMoveNames.GetMoveNames(Skills.currentWeaponSkill));

    }

    void SetNewElementalSkills() {
        elementalSkills = ParseMoveNames.GetMoveList(ParseMoveNames.GetMoveNames(Skills.currentElementalSkill));
    }
    void PhysicalAttackEnemy(Move move) {
        EnemyStatistics.totalCurrentHP[0] -= move.GetPower() * PlayerStatistics.totalStats["strength"] / EnemyStatistics.allEnemyStats[0]["defense"]; 
    }
    void ElementalAttackEnemy (Move move) {
        EnemyStatistics.totalCurrentHP[0] -= move.GetPower() * PlayerStatistics.totalStats["elemental_affinity"] / EnemyStatistics.allEnemyStats[0]["elemental_defense"]; 
    }
}
