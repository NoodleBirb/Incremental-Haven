
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{
    public bool showSkillList = false;
    public static bool isElementalTab;
    public static Dictionary<string, ISkillInterface> skillList;
    // The position on of the scrolling viewport
    public static ISkillInterface currentElementalSkill;
    public static ISkillInterface currentWeaponSkill;
    public static Dictionary<string, float> stats;
    public static bool isSkillsInitialized = false;
    public static float playerIncrementality;
    void Awake()
    {
        if (skillList == null) {
            skillList = new() {
                ["Alchemy"] = new Alchemy(),
                ["Crafting"] = new Crafting(),
                ["Endurance"] = new Endurance(),
                ["Farming"] = new Farming(),
                ["Fishing"] = new Fishing(),
                ["Forging"] = new Forging(),
                ["Magic"] = new Magic(),
                ["Malice"] = new Malice(),
                ["Mining"] = new Mining(),
                ["Scavenging"] = new Scavenging(),
                ["Ignition"] = new Ignition(),
                ["Woodcutting"] = new Woodcutting(),
                ["OneHandedCombat"] = new OneHandedCombat(),
                ["TwoHandedCombat"] = new TwoHandedCombat(),
                ["LightRanged"] = new LightRanged(),
                ["HeavyRanged"] = new HeavyRanged(),
                ["MakeshiftCombat"] = new MakeshiftCombat()
            };
            currentElementalSkill = skillList["Woodcutting"];
            stats = currentElementalSkill.GetStats();
            currentWeaponSkill = skillList["MakeshiftCombat"];
        }
        isElementalTab = true;
        isSkillsInitialized = true;
    }
    void Start() {
        GameObject.Find("Equipped Skill Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/SkillSprites/" + currentElementalSkill.GetName());
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

    static void FillSkillsWindow() {
        foreach (Transform skill in GameObject.Find("Skill List Content").transform) {
            Destroy (skill.gameObject);
        }
        foreach (ISkillInterface skill in skillList.Values) {
            GameObject skillBox = Instantiate(Resources.Load<GameObject>("UI/Skill Box"), GameObject.Find("Skill List Content").transform);
            if (isElementalTab) {
                if (skill.IsElementalSkill()) {
                    skillBox.transform.Find("Skill Box Image").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("UI/Images/SkillSprites/" + skill.GetName());
                    skillBox.transform.Find("Skill Box Level Value").GetComponent<TextMeshProUGUI>().text = "" + skill.GetLevel();
                    skillBox.GetComponent<Button>().onClick.AddListener(() => ChangeElementalSkill(skill));
                    skillBox.GetComponent<MouseOverSkill>().SetSkill(skill);
                } else {
                    Destroy (skillBox);
                }
            } else if (!isElementalTab) {
                if (!skill.IsElementalSkill()) {
                    skillBox.transform.Find("Skill Box Image").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("UI/Images/SkillSprites/" + skill.GetName());
                    skillBox.transform.Find("Skill Box Level Value").GetComponent<TextMeshProUGUI>().text = "" + skill.GetLevel();
                    skillBox.GetComponent<Button>().interactable = false;
                    skillBox.GetComponent<MouseOverSkill>().SetSkill(skill);
                } else {
                    Destroy (skillBox);
                }
            }
            
        }
    }
    public static void UpdateElementalSkillStats() {
        stats = currentElementalSkill.GetStats();
    }

    public static void ChangeElementalSkill(ISkillInterface skill) {
        currentElementalSkill = skill;
        if (GameObject.Find("Skill List Canvas") != null && isElementalTab) {
            GameObject.Find("Equipped Skill Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/SkillSprites/" + skill.GetName());
        }
        stats = skill.GetStats();
        PlayerStatistics.UpdateStats();
    }
    public static void ChangeWeaponSkill(ISkillInterface skill) {
        currentWeaponSkill = skill;
        if (GameObject.Find("Skill List Canvas") != null && !isElementalTab) {
            GameObject.Find("Equipped Skill Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/SkillSprites/" + skill.GetName());
        }
    }
    public void ElementalTab() {
        if (!isElementalTab){
            isElementalTab = true;
            GameObject.Find("Equipped Skill Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/SkillSprites/" + currentElementalSkill.GetName());
            FillSkillsWindow();
        }
    }
    public void WeaponTab() {
        if (isElementalTab) {
            isElementalTab = false;
            GameObject.Find("Equipped Skill Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/SkillSprites/" + currentWeaponSkill.GetName());
            FillSkillsWindow();
        }
    }
}
