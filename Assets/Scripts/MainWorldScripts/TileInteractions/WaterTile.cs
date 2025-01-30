using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaterTile : MonoBehaviour, InteractableObject
{
    private readonly int personalGUIHeight = 50;
    private int waterTime;
    public bool isFished;
    GameObject Player;
    
    void Start() {
        isFished = false;
        waterTime = 0;
        PlayerMovement.ResetActions += StopCuttingTree;
        Player = GameObject.Find("Player");
    }

    public void CreateOptions(float previousHeight) {
        GameObject interactionContainer = GameObject.Find("Interaction Container");
        
        GameObject interactButton = GameObject.Instantiate(Resources.Load<GameObject>("UI/Interaction Menu Button"));
        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Fish";
        interactButton.GetComponent<Button>().onClick.AddListener(() => BeginFishing());
        interactButton.transform.SetParent(interactionContainer.transform);
        interactButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -previousHeight);

        interactionContainer.GetComponent<RectTransform>().sizeDelta = interactionContainer.GetComponent<RectTransform>().sizeDelta + new Vector2(0, interactButton.GetComponent<RectTransform>().sizeDelta.y);
    }

    void BeginFishing() {
        InteractableObject.ResetGUI();
        Vector2Int pos = GetComponentInParent<BasicTile>().pos;
        if (Vector3.Distance(Player.transform.position, new(pos.x, 0, pos.y)) > 1){
            Player.GetComponent<PlayerMovement>().BeginMovement(transform.parent.gameObject);
        }
        if (!isFished && Equipment.GetEquippedItems()["Weapon Slot"] != null && Equipment.GetEquippedItems()["Weapon Slot"].GetSpecificFunctions()["is_fishing_rod"]) {
            StartCoroutine(Fish());
        }
    }

    public int GetGUIHeight() {
        return personalGUIHeight;
    }


    IEnumerator Fish() {
        while (Player.GetComponent<PlayerMovement>().movementPath.Count != 0) {
            yield return null;
        }
        waterTime += 1;
        if (waterTime == 30) {
            Skills.skillList["Fishing"].IncreaseEXP(20);
            StopCoroutine(Fish());
            waterTime = 0;
            isFished = true;
        }
        yield return new WaitForSeconds(.1f);
    }

    public void InteractWith() {
        if (!isFished && Equipment.GetEquippedItems()["Weapon Slot"] != null && Equipment.GetEquippedItems()["Weapon Slot"].GetSpecificFunctions().ContainsKey("is_fishing_rod"))  {
            StartCoroutine(Fish());
        }
    }
    public void StopCuttingTree() {
        StopCoroutine(Fish());
        waterTime = 0;

    }
}