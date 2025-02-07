using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EXPGainPopup : MonoBehaviour
{
    int popupTime;
    // Start is called before the first frame update
    void Start() {
        popupTime = 0;
        StartCoroutine(MoveUpThenVanish());

    }

    IEnumerator MoveUpThenVanish() {
        while (popupTime != 150) {
            popupTime += 1;
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(gameObject.GetComponent<RectTransform>().anchoredPosition.x, gameObject.GetComponent<RectTransform>().anchoredPosition.y + 5);
            yield return new WaitForSeconds(.01f);
        }
        GameObject.Destroy(gameObject);   
    }

    public static void CreateEXPGain(string name, int gain, int current, int needed) {
        GameObject expGain = GameObject.Instantiate(Resources.Load<GameObject>("UI/EXP Gain Popup"), GameObject.Find("EXP Gain Container").transform);
        expGain.transform.Find("EXP Gain Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/" + name);
        expGain.transform.Find("EXP Gain Value").GetComponent<TextMeshProUGUI>().text = "+" + gain;
        expGain.transform.Find("EXP Gain Percentage").GetComponent<TextMeshProUGUI>().text = (current / needed) + "%";
    }
}
