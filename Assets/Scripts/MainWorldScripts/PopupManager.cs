using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PopupManager : MonoBehaviour {

    public static void AddPopup(string name, string text) {
        GameObject popup = GameObject.Instantiate(Resources.Load<GameObject>("UI/Popup"), GameObject.Find("Popup Container").transform);
        popup.transform.Find("Popup Name").GetComponent<TextMeshProUGUI>().text = name + ":";
        popup.transform.Find("Popup Text").GetComponent<TextMeshProUGUI>().text = text;
        popup.transform.Find("Close Button").GetComponent<Button>().onClick.AddListener(() => GameObject.Destroy(popup));
    }
}