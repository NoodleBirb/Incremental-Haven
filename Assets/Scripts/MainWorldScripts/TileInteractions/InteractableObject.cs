using UnityEngine;

public interface InteractableObject {

    void CreateOptions(int previousHeight, Vector2 clickPos, int totalGUIWidth);

    int GetGUIHeight();
    void InteractWith();
}