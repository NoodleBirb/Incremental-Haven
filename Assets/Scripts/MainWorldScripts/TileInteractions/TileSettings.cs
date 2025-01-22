using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileSettings : MonoBehaviour
{

    // For usage in pathfinding.
    public bool walkable = true;
    // For logic with GUIBoxes
    public GameObject heldObject = null;

    public void GuiOptions () {

        GameObject interactionCanvas = GameObject.Find("Tile Interaction");
        GameObject interactionContainer = GameObject.Find("Interaction Container");
        RectTransform interactionContainerRT = interactionContainer.GetComponent<RectTransform>();

        GameObject interactButton = GameObject.Instantiate(Resources.Load<GameObject>("UI/Interaction Menu Button"));
        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Walk Towards";
        Debug.Log($"Listener added. Listener count: {interactButton.GetComponent<Button>().onClick.GetPersistentEventCount()}");
        interactButton.GetComponent<Button>().onClick.AddListener(() => WalkTowards());
        Debug.Log($"Listener added. Listener count: {interactButton.GetComponent<Button>().onClick.GetPersistentEventCount()}");
        interactButton.transform.SetParent(interactionContainer.transform);
        interactButton.GetComponent<RectTransform>().anchoredPosition = new Vector2 (0, 0);
        interactionContainer.GetComponent<RectTransform>().sizeDelta = new(interactButton.GetComponent<RectTransform>().rect.width, interactButton.GetComponent<RectTransform>().rect.height);
        
            
        if (heldObject != null) {
            InteractableObject interactable = heldObject.GetComponent<InteractableObject>();
            interactable?.CreateOptions(interactButton.GetComponent<RectTransform>().rect.height);
        }
        interactionContainerRT.anchoredPosition = interactionCanvas.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition) + new Vector3(interactionContainerRT.rect.width / 2, -interactionContainerRT.rect.height / 2, 0f);
        interactionCanvas.GetComponent<Canvas>().enabled = true;
    }

    void WalkTowards() {
        InteractableObject.ResetGUI();
        GameObject.Find("Player").GetComponent<PlayerMovement>().BeginMovement(gameObject);
    }
}