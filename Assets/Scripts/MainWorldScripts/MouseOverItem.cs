// from https://discussions.unity.com/t/onmouseover-ui-button-c/166886/2 by user AP124526435
// Edited for my own use
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Item item;
    private bool mouse_over = false;

    void Update() {
        if (mouse_over && RectTransformUtility.ScreenPointToWorldPointInRectangle(GameObject.Find("User Interface").GetComponent<RectTransform>(), Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main, out Vector3 newPoint)) {
            GameObject.Find("Tooltip").GetComponent<RectTransform>().position = newPoint;
        }
    }

    public void SetItem(Item item) {
        this.item = item;
    }
    void UpdateTooltipStats() {
        GameObject.Find("Item Name Tooltip").GetComponent<TextMeshProUGUI>().text = item.GetName() + "";
        GameObject.Find("Description Tooltip").GetComponent<TextMeshProUGUI>().text = item.GetStrength() + "";
        GameObject.Find("Strength Value Tooltip").GetComponent<TextMeshProUGUI>().text = item.GetStrength() + "";
        GameObject.Find("Speed Value Tooltip").GetComponent<TextMeshProUGUI>().text = item.GetSpeed() + "";
        GameObject.Find("Resistance Value Tooltip").GetComponent<TextMeshProUGUI>().text = item.GetResistance() + "";
        GameObject.Find("Mana Value Tooltip").GetComponent<TextMeshProUGUI>().text = item.GetMana() + "";
        GameObject.Find("Defense Value Tooltip").GetComponent<TextMeshProUGUI>().text = item.GetDefense() + "";
        GameObject.Find("Elemental Defense Value Tooltip").GetComponent<TextMeshProUGUI>().text = item.GetElementalDefense() + "";
        GameObject.Find("Elemental Affinity Value Tooltip").GetComponent<TextMeshProUGUI>().text = item.GetElementalAffinity() + "";
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        UpdateTooltipStats();
        GameObject.Find("Tooltip Canvas").GetComponent<Canvas>().enabled = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        GameObject.Find("Tooltip Canvas").GetComponent<Canvas>().enabled = true;
    }
}
