// from https://discussions.unity.com/t/onmouseover-ui-button-c/166886/2 by user AP124526435
// Edited for my own use
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverSkill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    bool specificMouseOver = false;
    RectTransform tooltipCanvasRect;
    ISkillInterface skill;
    Transform tooltip;

    void Update() {
        if ( GameObject.Find("Skill List Tooltip Canvas").GetComponent<Canvas>().enabled && specificMouseOver) {
            GameObject.Find("Skill List Tooltip Container").GetComponent<RectTransform>().anchoredPosition = tooltipCanvasRect.InverseTransformPoint(Input.mousePosition) + new Vector3(tooltip.gameObject.GetComponent<RectTransform>().rect.width, -tooltip.gameObject.GetComponent<RectTransform>().rect.height, 0f);
            UpdateTooltipStats();
        }
    }
    public void SetSkill(ISkillInterface skill) {
        this.skill = skill;
        tooltipCanvasRect = GameObject.Find("Skill List Tooltip Canvas").GetComponent<RectTransform>();
    }
    void UpdateTooltipStats() {
        foreach (Transform tooltip in GameObject.Find("Skill List Tooltip Container").transform) {
            Destroy(tooltip.gameObject);
        }
        if (skill.IsElementalSkill()) {
            tooltip = Instantiate(Resources.Load<GameObject>("UI/Skill Tooltip"), GameObject.Find("Skill List Tooltip Container").transform).transform;
            tooltip.Find("Skill Name Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetName() + "";
            tooltip.Find("EXP Gained Tooltip Value").GetComponent<TextMeshProUGUI>().text = skill.GetEXP() + "";
            tooltip.Find("EXP Until Tooltip Value").GetComponent<TextMeshProUGUI>().text = skill.GetThreshold() + "";
            tooltip.Find("Skill Level Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetLevel() + "";
            tooltip.Find("Strength Value Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetStats()["strength"] + "";
            tooltip.Find("Speed Value Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetStats()["speed"] + "";
            tooltip.Find("Mana Value Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetStats()["mana"] + "";
            tooltip.Find("Resistance Value Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetStats()["resistance"] + "";
            tooltip.Find("Defense Value Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetStats()["defense"] + "";
            tooltip.Find("Elemental Defense Value Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetStats()["elemental_defense"] + "";
            tooltip.Find("Elemental Affinity Value Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetStats()["elemental_affinity"] + "";
        } else {
            tooltip = Instantiate(Resources.Load<GameObject>("UI/Skill Tooltip Minimal"), GameObject.Find("Skill List Tooltip Container").transform).transform;
            tooltip.Find("Skill Name Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetName() + "";
            tooltip.Find("EXP Gained Tooltip Value").GetComponent<TextMeshProUGUI>().text = skill.GetEXP() + "";
            tooltip.Find("EXP Until Tooltip Value").GetComponent<TextMeshProUGUI>().text = skill.GetThreshold() + "";
            tooltip.Find("Skill Level Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetLevel() + "";
        }
    }
    public void OnPointerEnter(PointerEventData eventData) {
        specificMouseOver = true;
        UpdateTooltipStats();
        GameObject.Find("Skill List Tooltip Canvas").GetComponent<Canvas>().enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        specificMouseOver = false;
        GameObject.Find("Skill List Tooltip Canvas").GetComponent<Canvas>().enabled = false;
    }
}
