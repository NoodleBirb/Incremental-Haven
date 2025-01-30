using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;


public class PopupManager : MonoBehaviour {

    public static void AddPopup(string name, string text) {
        GameObject popup = GameObject.Instantiate(Resources.Load<GameObject>("UI/Popup"));
        popup.transform.Find("Popup Name").GetComponent<TextMeshProUGUI>().text = name + ":";
        popup.transform.Find("Popup Text").GetComponent<TextMeshProUGUI>().text = text;
        popup.transform.Find("Close Button").GetComponent<Button>().onClick.AddListener(() => GameObject.Destroy(popup));
        popup.transform.SetParent(GameObject.Find("Popup Container").transform);
    }
}