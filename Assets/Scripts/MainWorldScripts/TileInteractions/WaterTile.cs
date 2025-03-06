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
    Coroutine cor;
    GameObject Player;
    
    void Start() {
        isFished = false;
        waterTime = 0;
        cor = null;
        PlayerMovement.ResetActions += StopFishing;
        Player = GameObject.Find("Player");
    }

    public void CreateOptions(float previousHeight) {
        GameObject interactionContainer = GameObject.Find("Interaction Container");
        
        GameObject interactButton = GameObject.Instantiate(Resources.Load<GameObject>("UI/Interaction Menu Button"), interactionContainer.transform);
        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Fish";
        interactButton.GetComponent<Button>().onClick.AddListener(() => BeginFishing());
        interactButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -previousHeight);

        interactionContainer.GetComponent<RectTransform>().sizeDelta = interactionContainer.GetComponent<RectTransform>().sizeDelta + new Vector2(0, interactButton.GetComponent<RectTransform>().sizeDelta.y);
    }

    void BeginFishing() {
        InteractableObject.ResetGUI();
        Vector2Int pos = GetComponentInParent<BasicTile>().pos;
        if (Vector3.Distance(Player.transform.position, new(pos.x, 0, pos.y)) > 1){
            Player.GetComponent<PlayerMovement>().BeginInteractionMovement(transform.parent.gameObject);
        }
        if (!isFished && Equipment.GetEquippedItems()["Weapon Slot"] != null && Equipment.GetEquippedItems()["Weapon Slot"].GetSpecificFunctions()["is_fishing_rod"]) {
            Debug.Log("Started fishing coroutine");
            cor = StartCoroutine(Fish());
        }
    }

    public int GetGUIHeight() {
        return personalGUIHeight;
    }


    IEnumerator Fish() {
        while (Player.GetComponent<PlayerMovement>().movementPath.Count != 0) {
            
            yield return null;
        }
        while (waterTime != 30) {
           // Debug.Log(waterTime);
            waterTime += 1;
            yield return new WaitForSeconds(.1f);
        }
        EXPGainPopup.CreateEXPGain("Fishing", 20, (int)Skills.skillList["Fishing"].GetEXP() + 20, (int)Skills.skillList["Fishing"].GetThreshold());
        Skills.skillList["Fishing"].IncreaseEXP(20);
        waterTime = 0;
        isFished = true;
    }

    public void InteractWith() {
        if (!isFished && Equipment.GetEquippedItems()["Weapon Slot"] != null && Equipment.GetEquippedItems()["Weapon Slot"].GetSpecificFunctions().ContainsKey("is_fishing_rod"))  {
            cor = StartCoroutine(Fish());
        }
    }
    public void StopFishing() {
        if (cor != null) {
            StopCoroutine(cor);
        }
        waterTime = 0;
    }
}