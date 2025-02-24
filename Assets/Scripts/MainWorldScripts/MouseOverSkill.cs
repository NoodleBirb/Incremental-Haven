// from https://discussions.unity.com/t/onmouseover-ui-button-c/166886/2 by user AP124526435
// Edited for my own use
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverSkill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    bool specificMouseOver = false;
    RectTransform tooltipRect;
    RectTransform tooltipCanvasRect;
    ISkillInterface skill;

    void Update() {
        if ( GameObject.Find("Skill List Tooltip Canvas").GetComponent<Canvas>().enabled && specificMouseOver) {
            GameObject.Find("Skill List Tooltip Container").GetComponent<RectTransform>().anchoredPosition = tooltipCanvasRect.InverseTransformPoint(Input.mousePosition) + new Vector3(tooltipRect.rect.width, -tooltipRect.rect.height, 0f);
            UpdateTooltipStats();
        }
    }
    public void SetSkill(ISkillInterface skill) {
        this.skill = skill;
        tooltipRect = GameObject.Find("Skill Tooltip").GetComponent<RectTransform>();
        tooltipCanvasRect = GameObject.Find("Skill List Tooltip Canvas").GetComponent<RectTransform>();
    }
    void UpdateTooltipStats() {
        tooltipRect.transform.Find("Skill Name Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetName() + "";
        tooltipRect.transform.Find("EXP Gained Tooltip Value").GetComponent<TextMeshProUGUI>().text = skill.GetEXP() + "";
        tooltipRect.transform.Find("EXP Until Tooltip Value").GetComponent<TextMeshProUGUI>().text = skill.GetThreshold() + "";
        tooltipRect.transform.Find("Skill Level Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetLevel() + "";
        tooltipRect.transform.Find("Strength Value Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetStats()["strength"] + "";
        tooltipRect.transform.Find("Speed Value Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetStats()["speed"] + "";
        tooltipRect.transform.Find("Mana Value Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetStats()["mana"] + "";
        tooltipRect.transform.Find("Resistance Value Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetStats()["resistance"] + "";
        tooltipRect.transform.Find("Defense Value Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetStats()["defense"] + "";
        tooltipRect.transform.Find("Elemental Defense Value Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetStats()["elemental_defense"] + "";
        tooltipRect.transform.Find("Elemental Affinity Value Tooltip").GetComponent<TextMeshProUGUI>().text = skill.GetStats()["elemental_affinity"] + "";
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
