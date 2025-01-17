
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{
    public bool showSkillList = false;
    public bool isElementalTab;
    public static Dictionary<string, ISkillInterface> skillList;
    // The position on of the scrolling viewport
    public static ISkillInterface currentElementalSkill;
    public static ISkillInterface currentWeaponSkill;
    public static Dictionary<string, float> stats;
    public static event Action OnSkillsInitialized;
    public static bool isSkillsInitialized = false;
    public static float playerIncrementality;
    void Start()
    {
        if (skillList == null) {
            skillList = new() {
                ["Woodcutting"] = new Woodcutting(),
                ["Fishing"] = new Fishing(),
                ["One Handed Weapon"] = new OneHandedCombat(),
            };
            currentElementalSkill = skillList["Woodcutting"];
            stats = currentElementalSkill.GetStats();
            currentWeaponSkill = skillList["One Handed Weapon"];
        }
        isElementalTab = true;
        isSkillsInitialized = true;
        GameObject.Find("Equipped Skill Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/" + currentElementalSkill.GetName());
        OnSkillsInitialized?.Invoke();
    }

    public static void UpdateIncrementality() {
        playerIncrementality = 0;
        foreach(ISkillInterface skill in skillList.Values) {
            playerIncrementality += skill.GetLevel();
        }
        playerIncrementality /= skillList.Count;
    }
    public void OpenOrCloseSkillsWindow() {
        bool isEnabled = GameObject.Find("Skill List Canvas").GetComponent<Canvas>().enabled;
        if (!isEnabled) {
            
            FillSkillsWindow();
            GameObject.Find("Skill List Canvas").GetComponent<Canvas>().enabled = true;
        } else {
            GameObject.Find("Skill List Canvas").GetComponent<Canvas>().enabled = false;
        }
        
    }

    void FillSkillsWindow() {
        foreach (Transform skill in GameObject.Find("Skill List Content").transform) {
            Destroy (skill.gameObject);
        }
        foreach (ISkillInterface skill in skillList.Values) {
            GameObject skillBox = Instantiate(Resources.Load<GameObject>("UI/Skill Box"));
            if (isElementalTab) {
                if (skill.IsElementalSkill()) {
                    skillBox.transform.Find("Skill Box Image").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("UI/Images/" + skill.GetName());
                    skillBox.transform.Find("Skill Box Level Value").GetComponent<TextMeshProUGUI>().text = "" + skill.GetLevel();
                    skillBox.GetComponent<Button>().onClick.AddListener(() => ChangeElementalSkill(skill));
                    skillBox.transform.SetParent(GameObject.Find("Skill List Content").transform);
                } else {
                    Destroy (skillBox);
                }
            } else if (!isElementalTab) {
                if (!skill.IsElementalSkill()) {
                    skillBox.transform.Find("Skill Box Image").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("UI/Images/" + skill.GetName());
                    skillBox.transform.Find("Skill Box Level Value").GetComponent<TextMeshProUGUI>().text = "" + skill.GetLevel();
                    skillBox.GetComponent<Button>().interactable = false;
                    skillBox.transform.SetParent(GameObject.Find("Skill List Content").transform);
                } else {
                    Destroy (skillBox);
                }
            }
            
        }
    }
    public static void UpdateElementalSkillStats() {
        stats = currentElementalSkill.GetStats();
    }

    static void ChangeElementalSkill(ISkillInterface skill) {
        currentElementalSkill = skill;
        GameObject.Find("Equipped Skill Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/" + skill.GetName());
        stats = skill.GetStats();
        PlayerStatistics.UpdateStats();
    }
    public void ElementalTab() {
        if (!isElementalTab){
            isElementalTab = true;
            FillSkillsWindow();
        }
    }
    public void WeaponTab() {
        if (isElementalTab) {
            isElementalTab = false;
            FillSkillsWindow();
        }
    }
}
