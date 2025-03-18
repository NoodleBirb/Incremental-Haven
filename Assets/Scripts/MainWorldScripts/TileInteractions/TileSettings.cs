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

        GameObject interactButton = GameObject.Instantiate(Resources.Load<GameObject>("UI/Interaction Menu Button"), interactionContainer.transform);
        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Walk Towards";
        interactButton.GetComponent<Button>().onClick.AddListener(() => WalkTowards());
            
        if (heldObject != null) {
            InteractableObject interactable = heldObject.GetComponent<InteractableObject>();
            interactable?.CreateOptions(interactButton.GetComponent<RectTransform>().rect.height);
        }
        interactionContainerRT.anchoredPosition = interactionCanvas.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition) + new Vector3(interactionContainerRT.rect.width / 2, -interactionContainerRT.rect.height / 2, 0f);
        interactionCanvas.GetComponent<Canvas>().enabled = true;
    }

    void WalkTowards() {
        InteractableObject.ResetGUI();
        GameObject.Find("Player").GetComponent<PlayerMovement>().BeginMovement(gameObject, gui: true);
    }
}