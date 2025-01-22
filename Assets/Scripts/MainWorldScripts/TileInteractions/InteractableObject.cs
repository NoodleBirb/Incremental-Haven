using UnityEngine;

public interface InteractableObject {

    void CreateOptions(float previousHeight);

    int GetGUIHeight();
    void InteractWith();

    public static void ResetGUI() {
        GameObject interactionCanvas = GameObject.Find("Tile Interaction");
        GameObject interactionContainer = GameObject.Find("Interaction Container");

        foreach (Transform interactableButton in interactionContainer.transform) {
            GameObject.Destroy(interactableButton.gameObject);
        }
        interactionCanvas.GetComponent<Canvas>().enabled = false;
    }
}