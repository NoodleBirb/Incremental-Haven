// from https://discussions.unity.com/t/onmouseover-ui-button-c/166886/2 by user AP124526435
// Edited for my own use
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    Item item;
    private static bool mouse_over = false;
    RectTransform tooltipRect;
    RectTransform tooltipCanvasRect;

    void Start() {
        tooltipRect = GameObject.Find("Tooltip").GetComponent<RectTransform>();
        tooltipCanvasRect = GameObject.Find("Tooltip Canvas").GetComponent<RectTransform>();
    }

    void Update() {
        if (item != null && mouse_over) {
            tooltipRect.anchoredPosition = tooltipCanvasRect.InverseTransformPoint(Input.mousePosition) + new Vector3(tooltipRect.rect.width, -tooltipRect.rect.height, 0f);
        }
    }

    public void SetItem(Item item) {
        this.item = item;
    }
    void UpdateTooltipStats() {
        tooltipRect.transform.Find("Item Name Tooltip").GetComponent<TextMeshProUGUI>().text = "" + item.GetName();
        tooltipRect.transform.Find("Description Tooltip").GetComponent<TextMeshProUGUI>().text = "" + item.GetDescription();
        tooltipRect.transform.Find("Strength Value Tooltip").GetComponent<TextMeshProUGUI>().text = "+" + item.GetStrength();
        tooltipRect.transform.Find("Speed Value Tooltip").GetComponent<TextMeshProUGUI>().text = "+" + item.GetSpeed();
        tooltipRect.transform.Find("Resistance Value Tooltip").GetComponent<TextMeshProUGUI>().text = "+" + item.GetResistance();
        tooltipRect.transform.Find("Mana Value Tooltip").GetComponent<TextMeshProUGUI>().text = "+" + item.GetMana();
        tooltipRect.transform.Find("Defense Value Tooltip").GetComponent<TextMeshProUGUI>().text = "+" + item.GetDefense();
        tooltipRect.transform.Find("Elemental Defense Value Tooltip").GetComponent<TextMeshProUGUI>().text = "+" + item.GetElementalDefense();
        tooltipRect.transform.Find("Elemental Affinity Value Tooltip").GetComponent<TextMeshProUGUI>().text = "+" + item.GetElementalAffinity();
    }
    public void OnPointerEnter(PointerEventData eventData) {
        if (item == null) {
            return;
        }
        mouse_over = true;
        UpdateTooltipStats();
        GameObject.Find("Tooltip Canvas").GetComponent<Canvas>().enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (item == null) {
            return;
        }
        mouse_over = false;
        GameObject.Find("Tooltip Canvas").GetComponent<Canvas>().enabled = false;
    }

    public static void ItemVanished() {
        mouse_over = false;
        GameObject.Find("Tooltip Canvas").GetComponent<Canvas>().enabled = false;
    }
}
