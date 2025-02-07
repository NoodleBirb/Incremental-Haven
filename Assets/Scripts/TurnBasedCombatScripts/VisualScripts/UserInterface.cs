using System.Collections;
using System.Collections.Generic;
using Defective.JSON;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    Rect swapSkillRect;
    bool openSwapSkill;

    List<Move> elementalSkills;
    List<Move> weaponSkills;

    Slider playerHPSlider;
    Slider enemyHPSlider;
    Slider playerManaSlider;
    // Start is called before the first frame update
    void Start()
    {
        float rectXPos = Screen.width / 2 + 20;
        float rectYPos = Screen.height / 2;
        float rectHeight = 40f;
        float rectWidth = Screen.width / 2 - 40;
        swapSkillRect = new(rectXPos, rectYPos + 3 * 50f, rectWidth, rectHeight);

        playerHPSlider = GameObject.Find("Player HP Slider").GetComponent<Slider>();
        playerHPSlider.maxValue = PlayerStatistics.totalStats["HP"];
        playerHPSlider.value = PlayerStatistics.currentHP;

        enemyHPSlider = GameObject.Find("Enemy HP Slider").GetComponent<Slider>();
        enemyHPSlider.maxValue = EnemyStatistics.allEnemyStats[0]["HP"];
        enemyHPSlider.value = EnemyStatistics.totalCurrentHP[0];

        playerManaSlider = GameObject.Find("Player Mana Slider").GetComponent<Slider>();
        playerManaSlider.maxValue = PlayerStatistics.totalStats["mana"];
        playerManaSlider.value = PlayerStatistics.currentMana;



        GameObject.Find("Elemental Skill Button").GetComponent<Button>().onClick.AddListener(() => OpenElementalSkill());
        GameObject.Find("Weapon Skill Button").GetComponent<Button>().onClick.AddListener(() => OpenWeaponSkill());

        GameObject.Find("Change Skill Button").GetComponent<Button>().onClick.AddListener(() => OpenChangeSkill());
        GameObject.Find("Run Button").GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("firstarea"));

        FillMoveLists();
    }

    public static void BeginTurn() {
        GameObject.Find("Player Choices Canvas").GetComponent<Canvas>().enabled = true;
    }

    void OpenWeaponSkill() {

        foreach (Transform transform in GameObject.Find("Weapon Skill Container").transform) {
            GameObject.Destroy(transform.gameObject);
        }

        int i = 0;
        GameObject.Find("Player Choices Canvas").GetComponent<Canvas>().enabled = false;
        GameObject weaponSkillCanvas = GameObject.Find("Weapon Skill Canvas");
        foreach (Move move in weaponSkills) {
            GameObject button = GameObject.Instantiate(Resources.Load<GameObject>("UI/Use Skill Button"), GameObject.Find("Weapon Skill Container").transform);
            button.GetComponent<Button>().onClick.AddListener(() => UseWeaponSkill(move));
            button.GetComponent<Button>().onClick.AddListener(() => weaponSkillCanvas.GetComponent<Canvas>().enabled = false);
            
            button.GetComponentInChildren<TextMeshProUGUI>().text = move.GetName();
            i++;
        }
        weaponSkillCanvas.GetComponent<Canvas>().enabled = true;
    }
    void UseWeaponSkill(Move move) {
        if (PlayerStatistics.currentMana >= move.GetManaCost()) {
            Debug.Log("I used move: " + move.GetName() + " which had: " + move.GetPower() + " power~!");
            PlayerStatistics.currentMana -= move.GetManaCost();
            playerManaSlider.value = PlayerStatistics.currentMana;
            PhysicalAttackEnemy(move);

            TurnDecider.NextTurn();
        } else {
            Debug.Log("Not enough mana!");
        }

    }

    void OpenElementalSkill() {
        foreach (Transform transform in GameObject.Find("Elemental Skill Container").transform) {
            GameObject.Destroy(transform.gameObject);
        }

        int i = 0;
        GameObject.Find("Player Choices Canvas").GetComponent<Canvas>().enabled = false;
        GameObject elementalSkillCanvas = GameObject.Find("Elemental Skill Canvas");
        foreach (Move move in elementalSkills) {
            GameObject button = GameObject.Instantiate(Resources.Load<GameObject>("UI/Use Skill Button"), GameObject.Find("Elemental Skill Container").transform);
            button.GetComponent<Button>().onClick.AddListener(() => UseElementalSkill(move));
            
            button.GetComponentInChildren<TextMeshProUGUI>().text = move.GetName();
            
            i++;
        }
        elementalSkillCanvas.GetComponent<Canvas>().enabled = true;
    }
    void UseElementalSkill(Move move) {
        if (PlayerStatistics.currentMana >= move.GetManaCost()) {
            Debug.Log("I used move: " + move.GetName() + " which had: " + move.GetPower() + " power~!");
            PlayerStatistics.currentMana -= move.GetManaCost();
            playerManaSlider.value = PlayerStatistics.currentMana;
            ElementalAttackEnemy(move);
            GameObject.Find("Elemental Skill Canvas").GetComponent<Canvas>().enabled = false;
            TurnDecider.NextTurn();
        } else {
            Debug.Log("Not enough mana!");
        }

    }

    void OpenChangeSkill() {
        
        foreach (Transform transform in GameObject.Find("Change Skill Container").transform) {
            GameObject.Destroy(transform.gameObject);
        }
        GameObject.Find("Player Choices Canvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Change Skill Canvas").GetComponent<Canvas>().enabled = true;
        foreach (ISkillInterface skill in Skills.skillList.Values) {
                if (skill.IsElementalSkill()) {
                    GameObject skillBox = Instantiate(Resources.Load<GameObject>("UI/Skill Box"), GameObject.Find("Change Skill Container").transform);

                    skillBox.transform.Find("Skill Box Image").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("UI/Images/" + skill.GetName());
                    skillBox.transform.Find("Skill Box Level Value").GetComponent<TextMeshProUGUI>().text = "" + skill.GetLevel();
                    skillBox.GetComponent<Button>().onClick.AddListener(() => SetNewElementalSkills(skill));
                }
            }
    }

    void FillMoveLists() {
        elementalSkills = ParseMoveNames.GetMoveList(ParseMoveNames.GetMoveNames(Skills.currentElementalSkill));
        weaponSkills = ParseMoveNames.GetMoveList(ParseMoveNames.GetMoveNames(Skills.currentWeaponSkill));

    }

    void SetNewElementalSkills(ISkillInterface skill) {
        Skills.ChangeElementalSkill(skill);
        elementalSkills = ParseMoveNames.GetMoveList(ParseMoveNames.GetMoveNames(Skills.currentElementalSkill));
        GameObject.Find("Change Skill Canvas").GetComponent<Canvas>().enabled = false;
        TurnDecider.NextTurn();
    }
    void PhysicalAttackEnemy(Move move) {
        EnemyStatistics.totalCurrentHP[0] -= move.GetPower() * PlayerStatistics.totalStats["strength"] / EnemyStatistics.allEnemyStats[0]["defense"]; 
        enemyHPSlider.value = EnemyStatistics.totalCurrentHP[0];
    }
    void ElementalAttackEnemy (Move move) {
        EnemyStatistics.totalCurrentHP[0] -= move.GetPower() * PlayerStatistics.totalStats["elemental_affinity"] / EnemyStatistics.allEnemyStats[0]["elemental_defense"];
        enemyHPSlider.value = EnemyStatistics.totalCurrentHP[0]; 
    }

    public static void CloseAllMenus() {
        GameObject.Find("Weapon Skill Canvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Elemental Skill Canvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Change Skill Canvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Player Choices Canvas").GetComponent<Canvas>().enabled = true;
    }
}
