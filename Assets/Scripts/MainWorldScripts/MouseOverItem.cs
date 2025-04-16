// from https://discussions.unity.com/t/onmouseover-ui-button-c/166886/2 by user AP124526435
// Edited for my own use
using System;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseOverItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    Item item;
    static MouseOverItem currentItem;
    static RectTransform tooltipRect;
    static RectTransform tooltipCanvasRect;
    static RectTransform materialTooltipRect;

    void Start() {
        tooltipCanvasRect = GameObject.Find("Tooltip Canvas").GetComponent<RectTransform>();
    }

    void Update() {
        if (item != null && currentItem == this) {
            if (item.IsMaterial()) {
                materialTooltipRect.anchoredPosition = tooltipCanvasRect.InverseTransformPoint(Input.mousePosition) + new Vector3(materialTooltipRect.rect.width, -materialTooltipRect.rect.height, 0f);
            } else {
                tooltipRect.anchoredPosition = tooltipCanvasRect.InverseTransformPoint(Input.mousePosition) + new Vector3(tooltipRect.rect.width, -tooltipRect.rect.height, 0f);
            }
        }
    }

    public void SetItem(Item item) {
        this.item = item;
    }
    void UpdateTooltipStats() {
        if (item.IsMaterial()) {
            materialTooltipRect.transform.Find("Item Name").GetComponent<TextMeshProUGUI>().text = UpperCaseFirstLetter(String.Join(" ", item.GetName().Split('_')));
            materialTooltipRect.transform.Find("Description Tooltip").GetComponent<TextMeshProUGUI>().text = "" + item.GetDescription();
        } else {
            tooltipRect.transform.Find("Item Name").GetComponent<TextMeshProUGUI>().text = UpperCaseFirstLetter(String.Join(" ", item.GetName().Split('_')));
            tooltipRect.transform.Find("Description Tooltip").GetComponent<TextMeshProUGUI>().text = "" + item.GetDescription();
            tooltipRect.transform.Find("Strength Value Tooltip").GetComponent<TextMeshProUGUI>().text = item.GetStrength() < 0 ? item.GetStrength() + "" : "+" + item.GetStrength();
            tooltipRect.transform.Find("Speed Value Tooltip").GetComponent<TextMeshProUGUI>().text = item.GetSpeed() < 0 ? "" + item.GetSpeed() : "+" + item.GetSpeed();
            tooltipRect.transform.Find("Resistance Value Tooltip").GetComponent<TextMeshProUGUI>().text = item.GetResistance() < 0 ? "" + item.GetResistance() : "+" + item.GetResistance();
            tooltipRect.transform.Find("Mana Value Tooltip").GetComponent<TextMeshProUGUI>().text = item.GetMana() < 0 ? "" + item.GetMana() : "+" + item.GetMana();
            tooltipRect.transform.Find("Defense Value Tooltip").GetComponent<TextMeshProUGUI>().text = item.GetDefense() < 0 ? "" + item.GetDefense() : "+" + item.GetDefense();
            tooltipRect.transform.Find("Elemental Defense Value Tooltip").GetComponent<TextMeshProUGUI>().text = item.GetElementalDefense() < 0 ? "" + item.GetElementalDefense() : "+" + item.GetElementalDefense();
            tooltipRect.transform.Find("Elemental Affinity Value Tooltip").GetComponent<TextMeshProUGUI>().text = item.GetElementalAffinity() < 0 ? "" + item.GetElementalAffinity() : "+" + item.GetElementalAffinity();
        }
    }
    string UpperCaseFirstLetter(string str) {
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        return textInfo.ToTitleCase(str);
    }
    public void OnPointerEnter(PointerEventData eventData) {
        if (item == null) {
            return;
        }
        currentItem = this;
        if (item.IsMaterial()) {
            materialTooltipRect = Instantiate(Resources.Load<GameObject>("UI/Tooltip Minimal"), tooltipCanvasRect.transform).GetComponent<RectTransform>();
        } else {
            tooltipRect = Instantiate(Resources.Load<GameObject>("UI/Tooltip"), tooltipCanvasRect.transform).GetComponent<RectTransform>();
        }
        UpdateTooltipStats();
        tooltipCanvasRect.gameObject.GetComponent<Canvas>().enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (item == null) {
            return;
        }
        currentItem = null;
        foreach (Transform tooltip in tooltipCanvasRect.transform) {
            Destroy(tooltip.gameObject);
        }
        GameObject.Find("Tooltip Canvas").GetComponent<Canvas>().enabled = false;
    }

    public static void ItemVanished() {
        currentItem = null;
        foreach (Transform tooltip in tooltipCanvasRect.transform) {
            Destroy(tooltip.gameObject);
        }
        GameObject.Find("Tooltip Canvas").GetComponent<Canvas>().enabled = false;
    }
}
