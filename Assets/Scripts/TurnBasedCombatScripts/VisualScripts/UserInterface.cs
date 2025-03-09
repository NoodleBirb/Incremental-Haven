using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Defective.JSON;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    List<Move> elementalSkills;
    List<Move> weaponSkills;

    Slider playerHPSlider;
    Slider enemyHPSlider;
    Slider playerManaSlider;
    // Start is called before the first frame update
    void Start()
    {
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
        GameObject.Find("Inventory Button").GetComponent<Button>().onClick.AddListener(() => OpenInventory());
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

            GameObject.Find("ScriptHolder").GetComponent<CombatAnimationhandler>().RunAnimation(move.GetName(), GameObject.Find("Player"));
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
            GameObject.Find("ScriptHolder").GetComponent<CombatAnimationhandler>().RunAnimation(move.GetName(), GameObject.Find("Player"));
        } else {
            Debug.Log("Not enough mana!");
        }

    }

    void OpenInventory() {
        GameObject.Find("Player Choices Canvas").GetComponent<Canvas>().enabled = false;
        Inventory.LoadCombatInventory();
    }

    void OpenChangeSkill() {
        GameObject.Find("Player Choices Canvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Change Skill Canvas").GetComponent<Canvas>().enabled = true;
        
        GameObject.Find("Strength Value Skill").GetComponent<TextMeshProUGUI>().text = Skills.currentElementalSkill.GetStats()["strength"] + "";
        GameObject.Find("Elemental Affinity Value Skill").GetComponent<TextMeshProUGUI>().text = Skills.currentElementalSkill.GetStats()["elemental_affinity"] + "";
        GameObject.Find("Speed Value Skill").GetComponent<TextMeshProUGUI>().text = Skills.currentElementalSkill.GetStats()["speed"] + "";
        GameObject.Find("Mana Value Skill").GetComponent<TextMeshProUGUI>().text = Skills.currentElementalSkill.GetStats()["mana"] + "";
        GameObject.Find("Defense Value Skill").GetComponent<TextMeshProUGUI>().text = Skills.currentElementalSkill.GetStats()["defense"] + "";
        GameObject.Find("Elemental Defense Value Skill").GetComponent<TextMeshProUGUI>().text = Skills.currentElementalSkill.GetStats()["elemental_defense"] + "";
        GameObject.Find("Resistance Value Skill").GetComponent<TextMeshProUGUI>().text = Skills.currentElementalSkill.GetStats()["resistance"] + "";
        GameObject.Find("Skill Change Name").GetComponent<TextMeshProUGUI>().text = Skills.currentElementalSkill.GetName();

        GameObject.Find("Change Skill Function Button").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Change Skill Function Button").GetComponent<Button>().onClick.AddListener(() => Debug.Log("already using this skill"));

        GameObject.Find("Right Skill Button").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Left Skill Button").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Right Skill Button").GetComponent<Button>().onClick.AddListener(() => RightSkillButton(Skills.currentElementalSkill));
        GameObject.Find("Left Skill Button").GetComponent<Button>().onClick.AddListener(() => LeftSkillButton(Skills.currentElementalSkill));
    }

    void RightSkillButton(ISkillInterface currentSkill) {
        List<string> skillDictList = new();
        foreach (ISkillInterface skill in Skills.skillList.Values) {
            if (skill.IsElementalSkill()) {
                skillDictList.Add(skill.GetName());
            }
        }
        skillDictList.Sort();
        int currentIndex = skillDictList.IndexOf(currentSkill.GetName());
        if (currentIndex + 1 == skillDictList.Count) {
            UpdateChangeSkill(Skills.skillList[skillDictList[0]]);
        } else {
            UpdateChangeSkill(Skills.skillList[skillDictList[currentIndex + 1]]);
        }
    }

    void LeftSkillButton(ISkillInterface currentSkill) {
        List<string> skillDictList = new();
        foreach (ISkillInterface skill in Skills.skillList.Values) {
            if (skill.IsElementalSkill()) {
                skillDictList.Add(skill.GetName());
            }
        }
        skillDictList.Sort();
        int currentIndex = skillDictList.IndexOf(currentSkill.GetName());
        if (currentIndex - 1 == -1) {
            UpdateChangeSkill(Skills.skillList[skillDictList[^1]]);
        } else {
            UpdateChangeSkill(Skills.skillList[skillDictList[currentIndex - 1]]);
        }
    }

    void UpdateChangeSkill(ISkillInterface updatedSkill) {
        GameObject.Find("Strength Value Skill").GetComponent<TextMeshProUGUI>().text = updatedSkill.GetStats()["strength"] + "";
        GameObject.Find("Elemental Affinity Value Skill").GetComponent<TextMeshProUGUI>().text = updatedSkill.GetStats()["elemental_affinity"] + "";
        GameObject.Find("Speed Value Skill").GetComponent<TextMeshProUGUI>().text = updatedSkill.GetStats()["speed"] + "";
        GameObject.Find("Mana Value Skill").GetComponent<TextMeshProUGUI>().text = updatedSkill.GetStats()["mana"] + "";
        GameObject.Find("Defense Value Skill").GetComponent<TextMeshProUGUI>().text = updatedSkill.GetStats()["defense"] + "";
        GameObject.Find("Elemental Defense Value Skill").GetComponent<TextMeshProUGUI>().text = updatedSkill.GetStats()["elemental_defense"] + "";
        GameObject.Find("Resistance Value Skill").GetComponent<TextMeshProUGUI>().text = updatedSkill.GetStats()["resistance"] + "";
        GameObject.Find("Skill Change Name").GetComponent<TextMeshProUGUI>().text = updatedSkill.GetName();

        GameObject.Find("Change Skill Function Button").GetComponent<Button>().onClick.RemoveAllListeners();
        if (updatedSkill.GetName() != Skills.currentElementalSkill.GetName()) {
            GameObject.Find("Change Skill Function Button").GetComponent<Button>().onClick.AddListener(() => SetNewElementalSkills(updatedSkill));
        } else {
            GameObject.Find("Change Skill Function Button").GetComponent<Button>().onClick.AddListener(() => Debug.Log("already using this skill"));
        }
        GameObject.Find("Right Skill Button").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Left Skill Button").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Right Skill Button").GetComponent<Button>().onClick.AddListener(() => RightSkillButton(updatedSkill));
        GameObject.Find("Left Skill Button").GetComponent<Button>().onClick.AddListener(() => LeftSkillButton(updatedSkill));
    }

    void FillMoveLists() {
        elementalSkills = ParseMoveNames.GetMoveList(ParseMoveNames.GetMoveNames(Skills.currentElementalSkill));
        weaponSkills = ParseMoveNames.GetMoveList(ParseMoveNames.GetMoveNames(Skills.currentWeaponSkill));

    }

    void SetNewElementalSkills(ISkillInterface skill) {
        Debug.Log("Actually changing skill!");
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
        GameObject.Find("Inventory Canvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Change Skill Canvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Player Choices Canvas").GetComponent<Canvas>().enabled = true;
    }
}
